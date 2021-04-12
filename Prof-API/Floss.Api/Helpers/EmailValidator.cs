using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Floss.Api.Helpers
{
    /// <summary>
    /// class for email verifications
    /// </summary>
	public static class EmailValidator
	{
        /// <summary>
        /// checks if email is a students brock email
        /// </summary>
        /// <param name="emailString">email</param>
        /// <returns>email address</returns>
		public static MailAddress ValidateStudentEmail(string emailString)
		{
			try
			{
				MailAddress email = new MailAddress(emailString);

				bool validLogin = Regex.IsMatch(email.User, "^[A-Za-z]{2}[0-9]{2}[A-Za-z]{2}\\z"); // regex to check if its an student email format

				bool validEmail = string.Equals(email.Host.ToLower(), "brocku.ca"); // check if its a brock email

				if (validLogin && validEmail)
					return email;

				return null;
			}
			catch (Exception e) // if error occurs
			{
				return null;
			}
		}
        /// <summary>
        /// checks if email is a brock email
        /// </summary>
        /// <param name="emailString">email</param>
        /// <returns>email address</returns>
        public static MailAddress ValidateBrockEmail(string emailString)
		{
			try
			{
				MailAddress email = new MailAddress(emailString);  

				bool validEmail = string.Equals(email.Host.ToLower(), "brocku.ca"); // check if its a brock email

				if (validEmail)
					return email;

				return null;
			}
			catch (Exception e) // if error occurs
			{
				return null;
			}
		}
	}
}
