using MediatR;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Application.UserCommandsOrQueries.Validators
{
    public class AddUserCommandValidator(IUsersRepository usersRepository)
    {
        // This class can be used to validate the AddUserCommand
        // For example, you can check if the User entity is valid before adding it
        // You can implement methods to validate properties of the User entity
        
        public async Task<bool> ValidateAsync(UsersEntity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            
            // Validate username
            await ValidateUsername(user.UserName);
            
            // Validate email
            ValidateEmail(user.Email);
            
            // Validate phone number
            ValidatePhone(user.Phone);
            
            return true; // Return true if all validations pass
        }
        
        private async Task ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be empty or whitespace only");
            }
            
            // Check if username contains spaces
            if (username.Contains(" "))
            {
                throw new ArgumentException("Username cannot contain spaces");
            }
            
            // Check if username already exists
            var existingUser = await usersRepository.GetUserByIdOrUserNameAsync(null, username);
            if (existingUser != null)
            {
                throw new ArgumentException("Username already Used");
            }
        }
        
        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty");
            }
            
            // Regex pattern for basic email validation
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new ArgumentException("Invalid email format");
            }
        }        
        private void ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("Phone number cannot be empty");
            }
            
            // Regex pattern for Indian phone numbers
            // Accepts: 10 digits, optionally preceded by +91 or 0
            // Examples: 9876543210, +919876543210, 09876543210, +91-9876543210
            var phonePattern = @"^((\+91)|0)?[6-9]\d{9}$";
            if (!Regex.IsMatch(phone, phonePattern))
            {
                throw new ArgumentException("Invalid phone number."); // Must be 10 digits starting with 6-9, optionally preceded by +91 or 0
            }
        }
    }
}
