using Microsoft.AspNetCore.Mvc;
using practice.DTOs;
using practice.Models;
using practice.Services;

namespace practice.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ICandidateService _candidateService;
        private readonly IAdminService _AdminService;
        

        public AuthController(IAuthService authService,ICandidateService candidateService, IAdminService adminService)
        {
            _candidateService = candidateService;
            _authService = authService;
            _AdminService = adminService;

        }

        // GET: Login Page
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect to appropriate dashboard
            if (User.Identity?.IsAuthenticated == true)
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
                return RedirectToDashboard(role);
            }

            return View();
        }


        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // 1. Validate the input fields (Email/Password format)
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            // 2. Attempt to authenticate via the service
            var result = await _authService.LoginAsync(loginDto);

            // 3. Check if the user exists/password matches
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(loginDto);
            }

            // 4. BLOCK LOGIN if the user is not verified by an Admin
            // This prevents "bad cookies" from being created for unapproved users
            if (!result.IsVerified)
            {
                ModelState.AddModelError(string.Empty, "Your account is pending admin verification. You cannot log in yet.");
                return View(loginDto);
            }

            // 5. Store token in cookie (Only reached if the user is verified)
            Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensure this is true if using HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(0.5)
            });

            //// 6. Store user info in session
            HttpContext.Session.SetString("UserId", result.UserId.ToString());
            HttpContext.Session.SetString("FullName", result.FullName);
            HttpContext.Session.SetString("Email", result.Email);
            HttpContext.Session.SetString("Role", result.Role);
            HttpContext.Session.SetString("IsVerified", result.IsVerified.ToString());

            TempData["SuccessMessage"] = $"Welcome back, {result.FullName}!";

            // 7. Redirect to the appropriate dashboard
            if (result.Role == "Candidate")
            {

                var candidate = await _candidateService.GetCandidateByUserIdServiceAsync(result.UserId);
                bool status = await _candidateService.ChangeStatusaftertime(candidate);

            }
            else if (result.Role=="Admin") {
            var elections = await _AdminService.ElectionTimeCheckerAsync();


            }
                return RedirectToDashboard(result.Role);
        }
        // GET: Voter Registration Page
        [HttpGet]
        public IActionResult VoterRegister()
        {
            return View();
        }

        // POST: Voter Registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VoterRegister(RegisterVoterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            var result = await _authService.RegisterVoterAsync(registerDto);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Registration failed. Email or Number or CNIC may already exist.");
                return View(registerDto);
            }

            TempData["SuccessMessage"] = "Registration successful! Please wait for admin verification before logging in.";
            return RedirectToAction(nameof(Login));
        }

        // GET: Candidate Registration Page
        [HttpGet]
        public IActionResult CandidateRegister()
        {
            return View();
        }

        // POST: Candidate Registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CandidateRegister(RegisterCandidateDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            var result = await _authService.RegisterCandidateAsync(registerDto);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Registration failed. Email or Voter ID may already exist.");
                return View(registerDto);
            }

            TempData["SuccessMessage"] = "Candidate registration successful! Please wait for admin approval before logging in.";
            return RedirectToAction(nameof(Login));
        }

        // Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction(nameof(Login));
        }

        // Helper method to redirect to appropriate dashboard
        private IActionResult RedirectToDashboard(string? role)
        {
            return role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Candidate" => RedirectToAction("Dashboard", "Candidate"),
                "Voter" => RedirectToAction("Dashboard", "Voter"),
                _ => RedirectToAction("Index", "Home")
            };
        }


    }
}
