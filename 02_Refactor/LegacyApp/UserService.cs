using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        public UserService()
        {
            _clientRepository = new ClientRepository();
        }
        public UserService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!HasValidInput(firstName, lastName, email))
            {
                return false;
            }
            
            int age = CalculateAge(dateOfBirth);
            if (age < 21)
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                user.CreditLimit = GetCreditLimit(user.LastName, user.DateOfBirth) * 2;
            }
            else
            {
                user.HasCreditLimit = true;
                user.CreditLimit = GetCreditLimit(user.LastName, user.DateOfBirth);
            }
            
            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
        
        private bool HasValidInput(string firstName, string lastName, string email)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            return true;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            return age;
        }

        private int GetCreditLimit(string lastName, DateTime dateOfBirth)
        {
            using (var userCreditService = new UserCreditService())
            {
                return userCreditService.GetCreditLimit(lastName, dateOfBirth);
            }
        }
    }
    

}
