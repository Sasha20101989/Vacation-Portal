using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vacation_Portal.MVVM.Models
{
    public class VacationStore
    {

        private readonly Department _department;
        private Lazy<Task> _initializeLazy;

        private readonly List<Settings> _userSettings;


        //#region Collections
        //public List<Shift> Shifts => _shifts;
        //public List<Part> Parts => _parts;
        //public List<Shop> Shops => _shops;
        //public List<Detail> Details => _details;
        //public List<Settings> Settings => _userSettings;
        //public IEnumerable<Production> Productions => _productions;
        //public IEnumerable<BBSSRWK> A020 => _A020;
        //public IEnumerable<BBSSRWK> A090 => _A090;
        //public IEnumerable<VehicleAchievement> VehicleAchievement => _vehicleAchievement;
        //public IEnumerable<ProductionForecast> ProductionsForecast => _productionsForecast;
        //public IEnumerable<Production> ProductionsDay => _productionsDay;
        //public IEnumerable<Production> ProductionsLate => _productionsLate;
        //public IEnumerable<Production> ProductionsNight => _productionsNight;
        //#endregion

        #region Actions
        public event Action<Settings> SettingsUILoad;
        //public event Action<Production> ProductionMade;
        //public event Action<Production> ProductionRelocate;
        //public event Action<List<Production>> ProductionsLoad;
        //public event Action<Settings> SettingsLoad;

        //public event Action<List<ProductionForecast>> ForecastsLoad;

        #endregion

        #region Constructor
        public VacationStore(Department department)
        {
            _department = department;
            _initializeLazy = new Lazy<Task>(Initialize);
            _userSettings = new List<Settings>();
            //_shifts = new List<Shift>();
            //_parts = new List<Part>();
            //_shops = new List<Shop>();
            //_details = new List<Detail>();

            //_A020 = new List<BBSSRWK>();
            //_A090 = new List<BBSSRWK>();
            //_vehicleAchievement = new List<VehicleAchievement>();
            //_productions = new List<Production>();
            //_productionsForecast = new List<ProductionForecast>();
            //_productionsDay = new List<Production>();
            //_productionsLate = new List<Production>();
            //_productionsNight = new List<Production>();
            //_addShopLogger = addShopLogger;
        }

        #endregion

        public async Task Load()
        {
            try
            {
                await _initializeLazy.Value;
            }
            catch (Exception)
            {
                _initializeLazy = new Lazy<Task>(Initialize);
                throw;
            }
        }
       
        public async Task Initialize()
        {
            await Task.Delay(100);
        }

        public void LoadSettings()
        {
            /*
             *Load uniq settings for user interface
             *Font Size
             *Color
            */
            IEnumerable<Settings> userSettings = _department.GetSettingsUI(Environment.UserName);
            _userSettings.Clear();
            _userSettings.AddRange(userSettings);

            if (_userSettings.Count == 1)
            {
                //OnSettingsLoad(_userSettings[0]);
                OnSettingsUILoad(_userSettings[0]);
            }
        }
        private void OnSettingsUILoad(Settings settings)
        {
            SettingsUILoad?.Invoke(settings);
        }


        //#region Манипуляции с производством
        //public async Task DeleteProduction(Production production)
        //{
        //    await _shop.DeleteProduction(production);
        //    _productions.Remove(production);
        //    _addShopLogger.LogInformation($@"Производство детали {production.Part.Name} в колличестве {production.Part}, которое запланированно {production.EndDate} удалено (здесь может быть кем)");
        //}

        //public async Task DeleteForecastProductions()
        //{
        //    await _shop.DeleteForecastProductions();
        //}
        //public async Task DeleteOrderProductions()
        //{
        //    await _shop.DeleteOrderProductions();
        //}

        //public async Task UpdateProduction(Production production)
        //{
        //    await _shop.UpdateProduction(production);
        //    _addShopLogger.LogInformation($@"Производство детали {production.Part.Name} в колличестве {production.Part}, которое запланированно {production.EndDate} обновлено (здесь может быть кем)");
        //}
        //public async Task AddProduction (Production production) {
        //    await _shop.AddProduction (production);
        //    if (production.Shift.Id==1) {
        //        _productionsDay.Add(production);
        //    }
        //    else if (production.Shift.Id == 2)
        //    {
        //        _productionsNight.Add(production);
        //    }
        //    else if(production.Shift.Id == 3)
        //    {
        //        _productionsLate.Add(production);
        //    }
        //    OnProductionAdd (production);
        //    _productions.Add(production);
        //    _addShopLogger.LogInformation ($@"Производство детали {production.Part.Name} в колличестве {production.Plan}, которое запланированно {production.EndDate} добавлено в конец списка (здесь может быть кем)");
        //}
        //#endregion

        //#region Добавление в базу данных из Excel
        //public async Task AddBBSSItemA020(BBSSRWK production)
        //{
        //    await _shop.AddBBSSRWKItemA020(production);
        //}

        //public async Task AddBBSSItemA090(BBSSRWK production)
        //{
        //    await _shop.AddBBSSRWKItemA090(production);
        //}

        //public async Task AddBBSSItem(BBSS bbss)
        //{
        //    await _shop.AddBBSSItem(bbss);
        //}

        //public async Task UpdateActualUsage(int actUsage,ProductionForecast currentEntry)
        //{
        //    await _shop.UpdateActualUsage(actUsage, currentEntry);
        //}
        //public async Task AddOrder(ProductionForecast currentEntry)
        //{
        //    await _shop.AddOrder(currentEntry);
        //}
        //public async Task AddBOMItem(BOM bom)
        //{
        //    await _shop.AddBOMItem(bom);
        //}
        //public async Task AddFAKEItem(FAKE fake)
        //{
        //    await _shop.AddFAKEItem(fake);
        //}
        //#endregion

        //#region FRC07 Calculation
        //public async Task UpdateDBActualBalance(int actualBalance, string complete, ProductionForecast forecastProductionBoardViewModel)
        //{
        //    await _shop.UpdateDBActualBalance(actualBalance, complete,forecastProductionBoardViewModel);
        //}
        //public async Task UpdateDBStockStart(int stockStart, ProductionForecast currentEntry)
        //{
        //    await _shop.UpdateDBStockStart(stockStart,currentEntry);
        //}
        //public async Task UpdateDBForecastOrder(int plannedOrder, ProductionForecast currentEntry)
        //{
        //    await _shop.UpdateDBForecastOrder(plannedOrder,currentEntry);
        //}
        //public async Task UpdateDBForecastStockStart(int stockStart, ProductionForecast currentEntry)
        //{
        //    await _shop.UpdateDBForecastStockStart(stockStart, currentEntry);
        //}
        //public async Task UpdateDBForecastStockEnd(int forecastStockEnd, ProductionForecast currentEntry)
        //{
        //    await _shop.UpdateDBForecastStockEnd(forecastStockEnd, currentEntry);
        //}
        //#endregion

        //private void OnProductionAdd (Production production) {
        //    ProductionMade?.Invoke (production);
        //}
        //private void OnProductionsLoad(List<Production> productions)
        //{
        //    ProductionsLoad?.Invoke(productions);
        //}
        //private void OnProductionRelocate(Production production)
        //{
        //    ProductionRelocate?.Invoke(production);
        //}
        //private void OnForecastsLoad(List<ProductionForecast> productionsForecast)
        //{
        //    ForecastsLoad?.Invoke(productionsForecast);
        //}
        //private void OnSettingsLoad(Settings settings)
        //{
        //    SettingsLoad?.Invoke(settings);
        //}



        //public async Task Initialize () {
        //    IEnumerable<Settings> userSettings = await _shop.GetUserSettings(Environment.UserName);
        //    _userSettings.Clear();
        //    _userSettings.AddRange(userSettings);      

        //    IEnumerable<BBSSRWK> a020Productions = await _shop.GetA020Productions();
        //    _A020.Clear();
        //    _A020.AddRange(a020Productions);

        //    IEnumerable<BBSSRWK> a090Productions = await _shop.GetA090Productions();
        //    _A090.Clear();
        //    _A090.AddRange(a090Productions);

        //    IEnumerable<VehicleAchievement> vehicleAchievement = await _shop.GetVehicleAchievement();
        //    _vehicleAchievement.Clear();
        //    _vehicleAchievement.AddRange(vehicleAchievement);

        //    IEnumerable<Production> productions = await _shop.GetAllProductions();
        //    _productions.Clear();
        //    _productions.AddRange(productions);

        //    IEnumerable<ProductionForecast> productionsForecast = await _shop.GetForecastProductions();
        //    _productionsForecast.Clear();
        //    _productionsForecast.AddRange(productionsForecast);
        //    OnForecastsLoad(_productionsForecast);

        //    IEnumerable<Production> productionsDay = await _shop.GetProductionsForShift(2);
        //    _productionsDay.Clear();
        //    _productionsDay.AddRange(productionsDay);
        //    OnProductionsLoad(_productionsDay);

        //    IEnumerable<Production> productionsNight = await _shop.GetProductionsForShift(1);
        //    _productionsNight.Clear();
        //    _productionsNight.AddRange(productionsNight);
        //    OnProductionsLoad(_productionsNight);

        //    IEnumerable<Production> productionsLate = await _shop.GetProductionsForShift(3);
        //    _productionsLate.Clear();
        //    _productionsLate.AddRange(productionsLate);
        //    OnProductionsLoad(_productionsLate);


        //    IEnumerable<Shift> shifts = await _shop.GetShifts();
        //    _shifts.Clear();
        //    _shifts.AddRange(shifts);

        //    IEnumerable<Part> parts = await _shop.GetParts();
        //    _parts.Clear();
        //    _parts.AddRange(parts);

        //    IEnumerable<Shop> shops = await _shop.GetAllShops();
        //    _shops.Clear();
        //    _shops.AddRange(shops);

        //    IEnumerable<Detail>details = await _shop.GetAllDetails();
        //    _details.Clear();
        //    _details.AddRange(details);

        //    if (_userSettings.Count > 0)
        //    {
        //        OnSettingsLoad(_userSettings[0]);
        //        OnSettingsFormatLoad(_userSettings[0]);
        //    }
        //}       
    }
}
