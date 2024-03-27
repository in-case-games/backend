using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities;
public class WithdrawStatus : BaseEntity
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [JsonIgnore]
    public UserHistoryWithdraw? HistoryWithdraw { get; set; }
}