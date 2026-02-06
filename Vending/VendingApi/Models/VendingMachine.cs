using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class VendingMachine
{
    public string MachineId { get; set; } = null!;

    public int SerialNumber { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public byte WorkModeId { get; set; }

    public int CompanyId { get; set; }

    public byte StatusId { get; set; }

    public byte PriorityId { get; set; }

    public byte PlaceId { get; set; }

    public int ModelId { get; set; }

    public bool HasCoin { get; set; }

    public bool HasBill { get; set; }

    public bool HasQr { get; set; }

    public bool HasNfc { get; set; }

    public TimeOnly StartHour { get; set; }

    public TimeOnly EndHour { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public short Timezone { get; set; }

    public decimal TotalIncome { get; set; }

    public DateTime InstallDate { get; set; }

    public DateTime LastMaintenanceDate { get; set; }

    public string RfidCashCollection { get; set; } = null!;

    public string RfidLoading { get; set; } = null!;

    public string RfidService { get; set; } = null!;

    public string KitOnlineId { get; set; } = null!;

    public byte CriticalThresholdTemplateId { get; set; }

    public byte NotificationTemplateId { get; set; }

    public string? Notes { get; set; }

    public string? Status { get; set; }

    public bool IsServed { get; set; }

    public decimal ContributedMoney { get; set; }

    public decimal CoinsChange { get; set; }

    public decimal BillsChange { get; set; }

    public bool IsConnection { get; set; }

    public bool HasHardwareProblems { get; set; }

    public bool IsEncanced { get; set; }

    public bool IsFilledUp { get; set; }

    public int TotallGoods { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Template CriticalThresholdTemplate { get; set; } = null!;

    public virtual ICollection<MachineOperator> MachineOperators { get; set; } = new List<MachineOperator>();

    public virtual ICollection<MachineProduct> MachineProducts { get; set; } = new List<MachineProduct>();

    public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();

    public virtual Model Model { get; set; } = null!;

    public virtual Template NotificationTemplate { get; set; } = null!;

    public virtual MachinePlace Place { get; set; } = null!;

    public virtual ServicePriority Priority { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual Status StatusNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual WorkMode WorkMode { get; set; } = null!;
}
