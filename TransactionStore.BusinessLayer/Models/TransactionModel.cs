using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionStore.BusinessLayer.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TypeTransaction Type { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }
}
