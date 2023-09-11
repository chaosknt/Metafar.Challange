using System.ComponentModel.DataAnnotations;

namespace Metafar.Challange.Entities.Enum
{
    public enum AccountMovementsEnum
    {
        [Display(Name = "Extraccion")]
        Withdrawal,
        
        [Display(Name = "Deposito")]
        Deposit
    }
}