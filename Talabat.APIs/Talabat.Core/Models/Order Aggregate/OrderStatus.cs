using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Order_Aggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pending")]      //to mapped to string (Pending) in database table not 0
        Pending,
        [EnumMember(Value = "PaymentRecived")]
        PaymentRecived,
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed
    }
}
