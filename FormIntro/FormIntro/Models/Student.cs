using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FormIntro.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Please provide name")]
        [StringLength(10)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"^([1-9]|1[0-9]|2[0-4])-\d{5}-[1-3]@student\.aiub\.edu$", ErrorMessage = "Email must be in the format '21-45094-2@student.aiub.edu'.")]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        //[Range(1, 40, ErrorMessage = "Id must be 1 to 40")]
        [Emailcheck]
        [RegularExpression(@"^\d{2}-\d{5}-[1-3]$", ErrorMessage = "Id must be in the format '21-45094-2'. and should match email")]
        public string Id { get; set; }

        [Required(ErrorMessage = "enter date of birth")]
        [MinAge(20, ErrorMessage = "age must be at least 20 years")]
        public DateTime DateofBirth { get; set; }
    }

    public class Emailcheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var student = (Student)validationContext.ObjectInstance;
            string email = student.Email;
            string idMail = email.Substring(0, email.IndexOf('@'));
            string id = student.Id;

            if (id == idMail)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }

    public class MinAgeAttribute : ValidationAttribute
    {
        public int minAge;

        public MinAgeAttribute(int age)
        {
            minAge = age;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateofBirth)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - dateofBirth.Year;
                if (dateofBirth > today.AddYears(-age))
                {
                    age--;
                }
                if (age >= minAge)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return new ValidationResult("invalid date of birth.");
        }
    }
}