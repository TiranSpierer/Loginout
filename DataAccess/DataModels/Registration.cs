// UserManager/Domain/Registration.cs
// Created by Tiran Spierer
// Created at 04/01/2023
// Class propose:

namespace DataAccess.DataModels;

public class Registration : IEntity<Registration>
{
    public int    ProcedureId              { get; set; }
    public double XiphoidX  { get; set; }
    public double XiphoidY  { get; set; }
    public double XiphoidZ  { get; set; }
    public double RefX { get; set; }
    public double RefY { get; set; }
    public double RefZ { get; set; }


    #region Implementation of IEntity<in Registration>
    public void     CopyValuesTo(Registration entity)
    {
        entity.ProcedureId              = ProcedureId;
        entity.XiphoidX  = XiphoidX;
        entity.XiphoidY  = XiphoidY;
        entity.XiphoidZ  = XiphoidZ;
        entity.RefX = RefX;
        entity.RefY = RefY;
        entity.RefZ = RefZ;
    }

#endregion
}