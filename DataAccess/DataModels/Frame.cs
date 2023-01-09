// Demo_DatabaseApp/Domain/Frame.cs
// Created by Tiran Spierer
// Created at 05/01/2023
// Class propose:

using Domain.Models;
using System;

namespace DataAccess.DataModels;

public class Frame : IEntity<Frame>
{

    #region Public Properties

    public int Id { get; set; }

    public double RefX { get; set; }
    public double RefY { get; set; }
    public double RefZ { get; set; }

    public double TubeX { get; set; }
    public double TubeY { get; set; }
    public double TubeZ { get; set; }

    public DateTime Timestamp { get; set; }

    public         int       ProcedureId { get; set; }
    public virtual Procedure Procedure   { get; set; }

    #endregion

    #region Public Methods

#region Implementation of IEntity<in Frame>

    public void CopyValuesTo(Frame entity)
    {
        entity.RefX        = RefX;
        entity.RefY        = RefY;
        entity.RefZ        = RefZ;
        entity.TubeX       = TubeX;
        entity.TubeY       = TubeY;
        entity.TubeZ       = TubeZ;
        entity.Timestamp   = Timestamp;
        entity.ProcedureId = ProcedureId;
        entity.Procedure   = Procedure;
    }

#endregion

    #endregion

}