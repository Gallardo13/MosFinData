using System;
using System.Collections.Generic;

namespace GeoBudgetPrototypeWebApi.Models
{
    public class Contract
    {
        /// <summary>
        /// Id контракта
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Стоимость контракта
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string INN { get; set; }

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

        /// <summary>
        /// Номер контракта 
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Окато
        /// </summary>
        public string OKATO { get; set; }

        public List<string> OKPDs { get; set; }

    }
}
