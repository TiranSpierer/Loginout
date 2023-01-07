// UserManager/Domain/Procedure.cs
// Created by Tiran Spierer
// Created at 04/01/2023
// Class propose:

using System.Collections.Generic;

namespace DataAccess.DataModels;

public class Procedure : IEntity<Procedure>
{
    public int    Id        { get; set; }
    public int    PatientId { get; set; }
    public string UserId    { get; set; }

    public virtual Patient            Patient { get; set; }
    public virtual User               User    { get; set; }
    public virtual ICollection<Frame> Frames  { get; set; }

    #region Implementation of IEntity<in Procedure>

    public void CopyValuesTo(Procedure entity)
    {
        entity.Id        = Id;
        entity.PatientId = PatientId;
        entity.UserId    = UserId;
        entity.Patient   = Patient;
        entity.User      = User;
        entity.Frames = Frames;
    }

#endregion
}