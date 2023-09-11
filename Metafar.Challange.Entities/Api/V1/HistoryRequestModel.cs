using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Metafar.Challange.Entities.Api.V1
{
    public class HistoryRequestModel : BaseRequestModel
    {
        [Range(0, int.MaxValue)]
        public int? Start { get; set; }

        [Range(1, int.MaxValue)]
        public int? Length { get; set; } 

        public string? DateFrom { get; set; }

        public string? DateTo { get; set; }

        public bool ValidateDateFormat(string input)
        {
            string dateFormatPattern = @"^\d{4}-\d{2}-\d{2}$";

            if (Regex.IsMatch(input, dateFormatPattern))
            {
                if (DateTime.TryParse(input, out _))
                {
                    return true; 
                }
            }

            return false;
        }

        private string _validationMessage { get; set; }

        public string ValidationMessage() => _validationMessage;

        public DateTime? From() => ParseDate(this.DateFrom);

        public DateTime? To() => ParseDate(this.DateFrom);

        public bool IsValidModel()
        {
            var err = "La fecha {0} tiene un formato incorrecto";

            if (!string.IsNullOrWhiteSpace(DateFrom) && !this.ValidateDateFormat(DateFrom))
            {
                this._validationMessage = string.Format(err, "Desde");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(DateTo) && !this.ValidateDateFormat(DateTo))
            {
                this._validationMessage = string.Format(err, "Hasta");
                return false;
            }

            return DateValidation();
        }

        private bool DateValidation()
        {
            if(!string.IsNullOrWhiteSpace(DateFrom) && !string.IsNullOrWhiteSpace(DateTo))
            {
                var from = ParseDate(DateFrom);
                var to = ParseDate(DateTo);

                if(to < from )
                {
                    this._validationMessage = "La fecha Hasta no puede ser menor que la fecha Desde";
                    return false;
                }

            }

            return true;
        }

        private DateTime? ParseDate(string date)
            => string.IsNullOrWhiteSpace(date) ? null : DateTime.Parse(date);
    }
}
