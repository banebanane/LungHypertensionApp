﻿using System;
using System.Collections.Generic;
using LungHypertensionApp.Data.Entities;

namespace LungHypertensionApp.Data
{
    public interface ILungHypertensionRepository
    {
        #region General
        bool SaveAll();
        #endregion

        #region Institutions
        IEnumerable<Institution> GetAllInstitutions();
        Institution GetInstitutionById(string id); // get by name
        void SaveInstitution(Institution institution);
        void UpdateInstitution(Institution institution);
        void DeleteInstitution(Institution institution);
        #endregion

        #region Users
        StoreUser GetUserByEmail(string email);
        bool UpdateUserRole(StoreUser user, string role);
        void DeleteUser(StoreUser user);
        #endregion

        #region Patients
        int GetPatientMaxId();
        void SavePatient(Patient patient);
        Patient GetPatientById(int id);
        IEnumerable<PatientControll> GetPatientControlsByPatientId(int id);
        IEnumerable<Patient> GetAllPatientsByInstitution(string institutionName);
        IEnumerable<Patient> GetAllPatients();
        void UpdatePatient(Patient patient);
        bool UpdatePatientControlByPatientId(int id);
        void DeletePatient(Patient patient);
        #endregion

        #region Patients control

        void SavePatientControll(PatientControll patientControll);
        IEnumerable<PatientControll> GetAllControllsForPatient(int patientId);

        Dictionary<DateTime, string> GetAllControlsParamForPatientIdAndParam(int patientId, string param);

        #endregion
    }
}