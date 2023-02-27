using System;
using System.Data;
using System.Windows;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers {
    public class DependencyDetector : IDependencyDetector {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;

        private SqlTableDependency<HolidayDTO> _tableDependencyPlannedHoliday;
        private SqlTableDependency<VacationDTO> _tableDependencyPlannedVacation;
        private SqlTableDependency<PersonDTO> _tableDependencyPerson;

        public DependencyDetector(SqlDbConnectionFactory sqlDbConnectionFactory) {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }

        public bool StartDependencyPlannedHoliday() {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try {
                _tableDependencyPlannedHoliday = new SqlTableDependency<HolidayDTO>(database.ConnectionString, "tbd_Planned_Holidays");
                _tableDependencyPlannedHoliday.OnChanged += TableDependencyHoliday_OnChanged;
                _tableDependencyPlannedHoliday.Start();
                return true;
            } catch(Exception ex) {
                _ = MessageBox.Show(ex.Message);
            }
            return false;
        }
        public bool StopDependencyPlannedHoliday() {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try {
                if(_tableDependencyPlannedHoliday != null) {
                    _tableDependencyPlannedHoliday.Stop();
                    return true;
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        private void TableDependencyHoliday_OnChanged(object sender, RecordChangedEventArgs<HolidayDTO> e) {
            try {
                HolidayDTO changedEntity = e.Entity;
                switch(e.ChangeType) {
                    case ChangeType.None: {
                        //MessageBox.Show("ничего");
                    }
                    break;
                    case ChangeType.Delete: {
                        //MessageBox.Show("Могу удалить из интерфейса, так как из базы удалено");
                    }
                    break;
                    case ChangeType.Insert: {
                        //MessageBox.Show("Могу вставить в интерфейс, так как в базу вставлена запись");
                    }
                    break;
                    case ChangeType.Update: {
                        App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
                    }
                    break;
                };
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public bool StartDependencyPlannedVacation() {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try {
                _tableDependencyPlannedVacation = new SqlTableDependency<VacationDTO>(database.ConnectionString, "tbd_Planned_Vacations");
                _tableDependencyPlannedVacation.OnChanged += TableDependencyPlannedVacation_OnChanged;
                _tableDependencyPlannedVacation.Start();
                return true;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        public bool StopDependencyPlannedVacation() {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try {
                if(_tableDependencyPlannedVacation != null) {
                    _tableDependencyPlannedVacation.Stop();
                    return true;
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        private void TableDependencyPlannedVacation_OnChanged(object sender, RecordChangedEventArgs<VacationDTO> e) {
            try {
                VacationDTO changedEntity = e.Entity;
                switch(e.ChangeType) {
                    case ChangeType.None: {
                        //MessageBox.Show("ничего");
                    }
                    break;
                    case ChangeType.Delete: {
                        //MessageBox.Show("Могу удалить отпуск из интерфейса, так как из базы удален");
                    }
                    break;
                    case ChangeType.Insert: {
                        //MessageBox.Show("Могу вставить отпуск в интерфейс, так как в базу вставлена запись");
                    }
                    break;
                    case ChangeType.Update: {
                        //MessageBox.Show("Могу обновить отпуск в интерфейсе, так как в базе обновлен");
                    }
                    break;
                };
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public bool StartDependencyPerson() {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try {
                _tableDependencyPerson = new SqlTableDependency<PersonDTO>(database.ConnectionString, "tbd_Users");
                _tableDependencyPerson.OnChanged += TableDependencyPerson_OnChanged;
                _tableDependencyPerson.Start();
                return true;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        public bool StopDependencyPerson() {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try {
                if(_tableDependencyPerson != null) {
                    _tableDependencyPerson.Stop();
                    return true;
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        private void TableDependencyPerson_OnChanged(object sender, RecordChangedEventArgs<PersonDTO> e) {
            try {
                PersonDTO changedEntity = e.Entity;
                switch(e.ChangeType) {
                    case ChangeType.None: {
                        //MessageBox.Show("ничего");
                    }
                    break;
                    case ChangeType.Delete: {
                        //MessageBox.Show("Могу удалить пользователя из интерфейса, так как из базы удален");
                    }
                    break;
                    case ChangeType.Insert: {
                        //MessageBox.Show("Могу вставить пользователя в интерфейс, так как в базу вставлена запись");
                    }
                    break;
                    case ChangeType.Update: {
                        //MessageBox.Show("Могу обновить пользователя в интерфейсе, так как в базе обновлен");
                    }
                    break;
                };
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
