using System;
using System.Data;
using System.Windows;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class DependencyDetector : IDependencyDetector
    {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;

        private SqlTableDependency<HolidayDTO> tableDependencyPlannedHoliday;
        private SqlTableDependency<VacationDTO> tableDependencyPlannedVacation;
        private SqlTableDependency<PersonDTO> tableDependencyPerson;

        public DependencyDetector(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }

        public bool startDependencyPlannedHoliday()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                tableDependencyPlannedHoliday = new SqlTableDependency<HolidayDTO>(database.ConnectionString, "tbd_Planned_Holidays");
                tableDependencyPlannedHoliday.OnChanged += TableDependencyHoliday_OnChanged;
                tableDependencyPlannedHoliday.Start();
                return true;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        public bool stopDependencyPlannedHoliday()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                if(tableDependencyPlannedHoliday != null)
                {
                    tableDependencyPlannedHoliday.Stop();
                    return true;
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        private void TableDependencyHoliday_OnChanged(object sender, RecordChangedEventArgs<HolidayDTO> e)
        {
            try
            {
                HolidayDTO changedEntity = e.Entity;
                switch(e.ChangeType)
                {
                    case ChangeType.None:
                    {
                        MessageBox.Show("ничего");
                    }
                    break;
                    case ChangeType.Delete:
                    {
                        MessageBox.Show("Могу удалить из интерфейса, так как из базы удалено");
                    }
                    break;
                    case ChangeType.Insert:
                    {
                        MessageBox.Show("Могу вставить в интерфейс, так как в базу вставлена запись");
                    }
                    break;
                    case ChangeType.Update:
                    {
                        App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
                    }
                    break;
                };
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool startDependencyPlannedVacation()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                tableDependencyPlannedVacation = new SqlTableDependency<VacationDTO>(database.ConnectionString, "tbd_Planned_Vacations");
                tableDependencyPlannedVacation.OnChanged += TableDependencyPlannedVacation_OnChanged;
                tableDependencyPlannedVacation.Start();
                return true;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        public bool stopDependencyPlannedVacation()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                if(tableDependencyPlannedVacation != null)
                {
                    tableDependencyPlannedVacation.Stop();
                    return true;
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        private void TableDependencyPlannedVacation_OnChanged(object sender, RecordChangedEventArgs<VacationDTO> e)
        {
            try
            {
                VacationDTO changedEntity = e.Entity;
                switch(e.ChangeType)
                {
                    case ChangeType.None:
                    {
                        MessageBox.Show("ничего");
                    }
                    break;
                    case ChangeType.Delete:
                    {
                        MessageBox.Show("Могу удалить отпуск из интерфейса, так как из базы удален");
                    }
                    break;
                    case ChangeType.Insert:
                    {
                        MessageBox.Show("Могу вставить отпуск в интерфейс, так как в базу вставлена запись");
                    }
                    break;
                    case ChangeType.Update:
                    {
                        MessageBox.Show("Могу обновить отпуск в интерфейсе, так как в базе обновлен");
                    }
                    break;
                };
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool startDependencyPerson()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                tableDependencyPerson = new SqlTableDependency<PersonDTO>(database.ConnectionString, "tbd_Users");
                tableDependencyPerson.OnChanged += TableDependencyPerson_OnChanged;
                tableDependencyPerson.Start();
                return true;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        public bool stopDependencyPerson()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                if(tableDependencyPerson != null)
                {
                    tableDependencyPerson.Stop();
                    return true;
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        private void TableDependencyPerson_OnChanged(object sender, RecordChangedEventArgs<PersonDTO> e)
        {
            try
            {
                PersonDTO changedEntity = e.Entity;
                switch(e.ChangeType)
                {
                    case ChangeType.None:
                    {
                        MessageBox.Show("ничего");
                    }
                    break;
                    case ChangeType.Delete:
                    {
                        MessageBox.Show("Могу удалить пользователя из интерфейса, так как из базы удален");
                    }
                    break;
                    case ChangeType.Insert:
                    {
                        MessageBox.Show("Могу вставить пользователя в интерфейс, так как в базу вставлена запись");
                    }
                    break;
                    case ChangeType.Update:
                    {
                        MessageBox.Show("Могу обновить пользователя в интерфейсе, так как в базе обновлен");
                    }
                    break;
                };
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
