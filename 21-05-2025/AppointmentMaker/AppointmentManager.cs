using System;
using System.Collections.Generic;
using System.Globalization;
using AppointmentMaker.Models;
using AppointmentMaker.Interfaces;
using AppointmentMaker.Helpers;
using System.Linq;


namespace AppointmentMaker
{
    public class AppointmentManager
    {
        private readonly IAppointmentService _service;

        public AppointmentManager(IAppointmentService service)
        {
            _service = service;
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nAppointment Manager");
                Console.WriteLine("1. Add Appointment");
                Console.WriteLine("2. List Appointments");
                Console.WriteLine("3. Search Appointments");
                Console.WriteLine("4. Search Appointments with filters");
                Console.WriteLine("5. Exit");

                string choice = InputValidator.ReadOption("Select option:", new[] { "1", "2", "3", "4" ,"5" });

                switch (choice)
                {
                    case "1":
                        AddAppointment();
                        break;

                    case "2":
                        ListAppointments();
                        break;

                    case "3":
                        SearchAppointments();
                        break;
                    case "4":
                        SearchAppointmentsWithFilters();
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("Exiting application.");
                        break;
                }
            }
        }

        private void AddAppointment()
        {
            string name = InputValidator.ReadNonEmptyString("Enter patient name:");
            int age = InputValidator.ReadNonNegativeInt("Enter age:");
            DateTime date = InputValidator.ReadDate("Enter appointment date (dd-MM-yyyy):");
            string reason = InputValidator.ReadNonEmptyString("Enter reason for appointment:");

            var appointment = new Appointment
            {
                PatientName = name,
                Age = age,
                AppointmentDate = date,
                Reason = reason
            };

            _service.CreateAppointment(appointment);
            Console.WriteLine("Appointment added successfully.");
        }

        private void ListAppointments()
        {
            var appointments = _service.GetAllAppointments();
            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments found.");
                return;
            }

            Console.WriteLine("All Appointments:");
            foreach (var a in appointments)
            {
                Console.WriteLine($"[{a.Id}] {a.PatientName} | Age: {a.Age} | Date: {a.AppointmentDate:dd-MM-yyyy} | Reason: {a.Reason}");
            }
        }

        private void SearchAppointments()
        {
            Console.WriteLine("Search by:\n1. Name\n2. Date\n3. Age");
            string option = InputValidator.ReadOption("Choose search criteria:", new[] { "1", "2", "3" });

            List<Appointment> results = option switch
            {
                "1" => _service.FindAppointmentsByName(InputValidator.ReadNonEmptyString("Enter name to search:")),
                "2" => _service.FindAppointmentsByDate(InputValidator.ReadDate("Enter date (yyyy-MM-dd) to search:")),
                "3" => _service.FindAppointmentsByAge(InputValidator.ReadNonNegativeInt("Enter age to search:")),
                _ => new List<Appointment>()
            };

            if (results.Count == 0)
            {
                Console.WriteLine("No matching appointments found.");
                return;
            }

            Console.WriteLine("Search Results:");
            foreach (var a in results)
            {
                Console.WriteLine($"[{a.Id}] {a.PatientName} | Age: {a.Age} | Date: {a.AppointmentDate:dd-MM-yyyy} | Reason: {a.Reason}");
            }
        }


    private void SearchAppointmentsWithFilters()
    {
        Console.WriteLine("\n--- Search Appointments ---");

        string nameFilter;
        string dateInput;
        string minAgeInput;
        string maxAgeInput;

        // Loop until at least one input is provided
        do
        {
            nameFilter = InputValidator.ReadOptionalString("Enter patient name (leave empty to skip):");
            dateInput = InputValidator.ReadOptionalString("Enter appointment date (dd-MM-yyyy) (leave empty to skip):");
            minAgeInput = InputValidator.ReadOptionalString("Enter minimum age (leave empty to skip):");
            maxAgeInput = InputValidator.ReadOptionalString("Enter maximum age (leave empty to skip):");

            if (string.IsNullOrWhiteSpace(nameFilter) &&
                string.IsNullOrWhiteSpace(dateInput) &&
                string.IsNullOrWhiteSpace(minAgeInput) &&
                string.IsNullOrWhiteSpace(maxAgeInput))
            {
                Console.WriteLine("Please enter at least one filter to search appointments.");
            }
            else
            {
                break;
            }

        } while (true);

        DateTime? parsedDate = null;
        if (!string.IsNullOrWhiteSpace(dateInput))
        {
            if (DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                parsedDate = date;
            }
            else
            {
                Console.WriteLine("Invalid date format. Please use dd-MM-yyyy.");
                return;
            }
        }

        int? minAge = null;
        int? maxAge = null;

        if (!string.IsNullOrWhiteSpace(minAgeInput) && int.TryParse(minAgeInput, out int min))
            minAge = min;

        if (!string.IsNullOrWhiteSpace(maxAgeInput) && int.TryParse(maxAgeInput, out int max))
            maxAge = max;

        var results = _service.SearchAppointments(nameFilter, parsedDate, minAge, maxAge);

        if (!results.Any())
        {
            Console.WriteLine("No matching appointments found.");
            return;
        }

        Console.WriteLine("Search Results:");
        foreach (var a in results)
        {
            Console.WriteLine($"[{a.Id}] {a.PatientName} | Age: {a.Age} | Date: {a.AppointmentDate:dd-MM-yyyy} | Reason: {a.Reason}");
        }
    }

    }
}