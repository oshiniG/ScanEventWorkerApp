Scan Event Worker Application

Overview
This solution consists of two main projects:

- **ScanEventWorkerApp.Presentation.API**: The external Scan Event API simulation.
- **ScanEventWorkerApp.Presentation.Worker**: The worker application that consumes the Scan Event API and processes scan events.

 1. Run the Scan Event API
The API project simulates the external scan event source.

 2. Run the Scan Event Worker
The worker project fetches scan events from the API and processes them.

 **Assumptions**
The primary event types relevant for business logic are PICKUP and DELIVERY. 
Persisting scan events and last processed EventId to a local file is acceptable for this exercise. No database or external durable storage is required.


**Improvements**
Replace file-based persistence with a proper database
In a scaled environment with multiple worker instances, introduce distributed locking or coordination to avoid duplicated processing.
Integrate structured logging frameworks
For events failing processing repeatedly, implement a dead-letter queue or error queue for manual inspection and reprocessing.
Externalize configuration using environment variables or configuration servers for flexibility in different environments.
