# MLM User Registration and Referral Tracking System

A simple Multi-Level Marketing (MLM) registration and referral tracking module built using **ASP.NET MVC** and **SQL Server**.

---

## 🧾 Features

- ✅ User Registration (Full Name, Email, Password, Mobile Number, Sponsor ID)
- 🔐 Secure Password Hashing using BCrypt
- 🔗 Sponsor Validation (checks if Sponsor ID exists)
- 🌳 Referral Tree View (shows all users referred by a sponsor)
- 🧪 Server-side validation with clean error messages
- 📁 N-Tier Architecture (DAL, Model, Controller)

---

## 🛠 Tech Stack

- **Frontend**: Razor Views (ASP.NET MVC)
- **Backend**: ASP.NET MVC (.NET Framework 4.6.1)
- **Database**: SQL Server 2019
- **ORM**: ADO.NET with Stored Procedures
- **Password Security**: BCrypt.Net

---

## 📂 Folder Structure

```bash
MLM_task/
├── Controllers/
│   └── UserController.cs
├── DAL/
│   └── UserDAL.cs
├── Models/
│   └── RegisterDto.cs
│   └── User.cs
│   └── ReferralDto.cs
├── Views/
│   └── User/
│       ├── Register.cshtml
│       └── Referrals.cshtml
