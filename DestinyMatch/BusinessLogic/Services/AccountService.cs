using BusinessLogic.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;
using Repository.Models;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<Account> GetByEmailAsync(string email)
        {
            return await _accountRepository.GetByEmailAsync(email);
        }
        public async Task<bool> CreateAccountAsync(string email, string password)
        {
            var existAccount = await _accountRepository.GetByEmailAsync(email);
            if (email.IsNullOrEmpty() || existAccount is not null)
            {
                return false;
            }
            string hashedPassword = HashString(password);
            _accountRepository.Add(
                new Account
                {
                    Email = email,
                    Password = hashedPassword
                }
            );
            return true;
        }

        private string HashString(string input)//SHA-256 Algorithm (1 way)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public async Task<Account?> LoginByPassWord(string email, string password)
        {
            var existAccount = await _accountRepository.GetByEmailAsync(email);
            if (email.IsNullOrEmpty() || existAccount is null || password.IsNullOrEmpty())
            {
                return null;
            }
            string hashedPassword = HashString(password);

            if (!existAccount.Password.Equals(hashedPassword))
            {
                return null;
            }
            return existAccount;
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _accountRepository.GetAllAsync();
        }
        /*
         [HttpPost]
        [Route("api/login/google")]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken,
        new GoogleJsonWebSignature.ValidationSettings());

            // Extract the email from the payload
            var email = payload.Email;

            string username = "", password = "";
            bool needSignup = true, existEmail = false;
            User user;
            Teacher teacher;
            Staff staff;
            Admin admin;
            for (int role = 1; role <= 4; role++)
            {
                switch (role)
                {
                    case 1:
                        user = await _context.Users
                        .Include(u => u.Account)
                        .SingleOrDefaultAsync(usr => usr.Email.Equals(email));
                        existEmail = (user is not null);
                        username = (user is not null) ? user.Account.Username : string.Empty;
                        password = (user is not null) ? user.Account.Password : string.Empty;
                        break;
                    case 2:
                        teacher = await _context.Teachers
                        .Include(tch => tch.Account)
                        .SingleOrDefaultAsync(tch => tch.Email.Equals(email));
                        existEmail = (teacher is not null);
                        username = (teacher is not null) ? teacher.Account.Username : string.Empty;
                        password = (teacher is not null) ? teacher.Account.Password : string.Empty;
                        break;
                    case 3:
                        staff = await _context.Staff
                        .Include(stf => stf.Account)
                        .SingleOrDefaultAsync(stf => stf.Email.Equals(email));
                        existEmail = (staff is not null);
                        username = (staff is not null) ? staff.Account.Username : string.Empty;
                        password = (staff is not null) ? staff.Account.Password : string.Empty;
                        break;
                    case 4:
                        admin = await _context.Admins
                        .Include(adm => adm.Account)
                        .SingleOrDefaultAsync(adm => adm.Email.Equals(email));
                        existEmail = (admin is not null);
                        username = (admin is not null) ? admin.Account.Username : string.Empty;
                        password = (admin is not null) ? admin.Account.Password : string.Empty;
                        break;
                }
                //If exist then stop checking
                if (existEmail)
                {
                    needSignup = false;
                    break;
                }

            }//End loop

            //If not exist then create account
            if (needSignup)
            {
                username = email;
                password = Guid.NewGuid().ToString();
                await _context.Database.ExecuteSqlRawAsync("exec dbo.proc_signUpAccount @username = @p0, @password = @p1, @email = @p2", email, password, email);
            }

            //Then login
            var account = await _context.Accounts
                .AsNoTracking()
                .SingleOrDefaultAsync(acc => acc.Username.Equals(username) && acc.Password.Equals(password));

            HttpContext.Session.SetString("usersession", JsonSerializer.Serialize(account));
            await HttpContext.Session.CommitAsync();
            return RedirectToAction("Index", "Home");
        }
        */
    }
}
