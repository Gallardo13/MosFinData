using System;
using System.Collections.Generic;

namespace GeoBudgetPrototypeWebApi.Models
{
    public class Contract
    {
        /// <summary>
        /// Стоимость контракта
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Дата начала действия контракта
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия контракта 
        /// </summary>
        public DateTime DateFinish { get; set; }

        /// <summary>
        /// Id статуса 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Url контракта 
        /// </summary>
        public string Url { get; set; }

        public string OKPDs { get; set; }

    }
}
