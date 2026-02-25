# FinVue 🚀

![.NET 10](https://img.shields.io/badge/.NET%2010-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Vue 3](https://img.shields.io/badge/Vue%203-4FC08D?style=for-the-badge&logo=vuedotjs&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)

FinVue is a high-performance, full-stack financial dashboard designed to track mutual fund investments with real-time analytics. Built using a Hexagonal (Ports & Adapters) Architecture to ensure robust separation of concerns and database portability.

## Key Features :
Real-time Ledger: Full CRUD operations for asset management.

Intelligent Analytics: Live-computed stats for Portfolio Value, Avg NAV, and Top Performer.

Reactive Visualization: Dynamic asset allocation breakdown via Chart.js.

Midnight Green UI: WCAG-compliant high-contrast dark theme for financial analysis.

## Tech Stack :
Frontend: Vue 3 (Composition API), TypeScript, Tailwind CSS.

Backend: .NET 10 Web API, C#.

Database: PostgreSQL (Dockerized).

Architecture: Clean/Hexagonal Architecture with Repository Pattern.

## 🔄 The "Architectural Pivot" :
Due to an instruction set incompatibility with Oracle XE on Apple Silicon (ARM64), I leveraged the Hexagonal Architecture of the backend to swap the persistence layer to PostgreSQL. By implementing a new Repository interface, the core business logic remained untouched, demonstrating the power of decoupled design.

## Screenshot :

<img width="1353" height="841" alt="image" src="https://github.com/user-attachments/assets/6cf5e368-0d24-4821-8d5b-910cd9b012ff" />
