# FinancialBankV2 - Full-Stack Banking Simulation

This project is a personal Full-Stack banking application developed using modern **.NET technologies** and **Angular**. The application was built to demonstrate core banking features, **privacy-first AI integration**, and layered architecture principles (DDD).

## 🚀 Key Features

- **User Management:** Secure user login and registration utilizing the **ABP Framework's** built-in identity management module.
- **Account Panel:** Allows logged-in users to view their bank accounts and real-time balances.
- **Money Transfer:** Enables users to transfer funds to other accounts within the system using account numbers.
- **Transaction History:** Records all transfers as persistent `Transaction` entries in the database.
- **AI Assistant (Privacy-First):** Integrated with **Semantic Kernel & Local LLMs**, allowing users to ask natural language questions about their account balances without data leaving the local environment.
- **Transaction Receipt:** Generates a dynamic receipt page displaying transaction details (sender, receiver, amount, date) after every successful transfer.

## 🛠️ Tech Stack

### Backend
- **Framework:** ABP Framework v9.3.5, ASP.NET Core
- **Architecture:** Layered Architecture, Domain-Driven Design (DDD) Principles
- **API:** RESTful API
- **ORM:** Entity Framework Core
- **Database:** Microsoft SQL Server
- **AI Integration:** Microsoft Semantic Kernel (Local LLM Support)
- **Other:** AutoMapper, Dependency Injection, Unit of Work

### Frontend
- **Framework:** Angular
- **UI Library:** Bootstrap, FontAwesome
- **Language:** TypeScript

## ⚙️ Running the Project

Follow the steps below to run the project on your local machine:

1. **Database Setup:**
    - Update the `ConnectionStrings` section in the `appsettings.json` file with your own SQL Server configuration.
    - Run the `src/FinancialBankV2.DbMigrator` project to apply migrations and seed initial data.

2. **Running the Backend:**
    - Navigate to `src/FinancialBankV2.HttpApi.Host` and run the project using the `dotnet run` command.
    - The API will start running at `https://localhost:44363`.

3. **Running the Frontend:**
    - Navigate to the `angular` folder and install dependencies using the `yarn` command.
    - Start the frontend server with `ng serve`.
    - The application will be accessible at `http://localhost:4200`.

## 👤 Default User Credentials

You can use the following standard admin credentials to test the application:
- **Username:** `admin`
- **Password:** `1q2w3E*`

---

*This project was developed by Ömer Faruk YAZICI.*

<img width="1600" height="796" alt="image" src="https://github.com/user-attachments/assets/78f42b71-7319-4773-9735-ff1613a7b89e" />
<img width="1600" height="820" alt="image" src="https://github.com/user-attachments/assets/2c6b7f6c-503d-49fc-a236-e661e28949d1" />
<img width="1600" height="712" alt="image" src="https://github.com/user-attachments/assets/f1865308-0603-4f42-b359-fb31538e0678" />
<img width="1600" height="758" alt="image" src="https://github.com/user-attachments/assets/3df49695-d3c5-448d-be8f-0166bda3a316" />
<img width="1600" height="756" alt="image" src="https://github.com/user-attachments/assets/5ffbfafe-3847-42c0-8a24-eb66405ebc8b" />




