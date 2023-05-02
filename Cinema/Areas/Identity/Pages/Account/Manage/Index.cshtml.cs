// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Cinema.Utility;
using Cinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cinema.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Utente> _userManager;
        private readonly SignInManager<Utente> _signInManager;

        public IndexModel(
            UserManager<Utente> userManager,
            SignInManager<Utente> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }
        public string PhoneNumber { get; private set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            ///
            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "EMail")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Nome")]
            public string Nome { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Cognome")]
            public string Cognome { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Genere")]
            public string Sesso { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Data di nascita")]
            public DateOnly Nascita { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Indirizzo")]
            public string Residenza { get; set; }

            [Required]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Numero di Telefono")]
            public string PhoneNumber { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }

        private async Task LoadAsync(Utente user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            PhoneNumber = phoneNumber;

            Input = new InputModel
            {
                Nome = user.Nome,
                Cognome = user.Cognome,
                Nascita = user.Nascita,
                Residenza = user.Residenza,
                Sesso = user.Sesso
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }


            if (Input.Nome != user.Nome)
            {
                user.Nome = Input.Nome;
            }
            if (Input.Cognome != user.Cognome)
            {
                user.Cognome = Input.Cognome;
            }
            if (Input.Nascita != user.Nascita)
            {
                user.Nascita = Input.Nascita;
            }
            if (Input.Residenza != user.Residenza)
            {
                user.Residenza = Input.Residenza;
            }
            if (Input.Sesso != user.Sesso)
            {
                user.Sesso = Input.Sesso;
            }
            if (Input.Email != user.Email)
            {
                user.Email = Input.Email;
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
