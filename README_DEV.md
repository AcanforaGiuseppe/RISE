# RISE – Developer Documentation

## Overview
This document describes the technical architecture of the RISE platform.

## Stack
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap 5
- Authentication with Cookies
- Admin Area with Role-based Security

## Architecture
- /Controllers: Public MVC controllers
- /Areas/Admin: Admin panel (CRUD, approval flows, analytics)
- /Models: Entity models and ViewModels
- /Views: Razor UI
- /wwwroot: Static assets (css, js, images)

## Core Features
- News management
- Competition management
- Athlete registration with approval workflow
- User management with roles
- Retention and Geography analytics
- Social Feed integration
- Slider image upload system
- Dark/Light theme system

## Database Entities
- User
- Registration
- Competition
- News
- FAQ
- NewsletterSubscriber
- SocialPost

## Registration Flow
1. User submits form.
2. Stored in Registrations as Pending.
3. Admin approves.
4. Converted to User.
5. Registration removed.

## Security
- Admin authentication via cookie scheme
- Password hashing
- Role validation (Admin / User)

## Build & Run
1. Open solution in Visual Studio.
2. Restore NuGet packages.
3. Update connection string.
4. Run migrations.
5. Start IIS Express.