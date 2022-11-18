using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.MVVM.Models
{
   public class VacationSummary
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IUserProvider _userProvider;
        private readonly IDepartmentProvider _shopProvider;

        public VacationSummary(ISettingsProvider settingsProvider, IUserProvider userProvider, IDepartmentProvider shopProvider)
        {
            _settingsProvider = settingsProvider;
            _userProvider = userProvider;
            _shopProvider = shopProvider;
        }

        public async Task<IEnumerable<Settings>> GetSettingsUI(string user)
        {
            return await _settingsProvider.GetSettingsUI(user);
        }

        public async Task <IEnumerable<Person>> GetUser(string account)
        {
            return await _userProvider.GetUser(account);
        }

        public async Task<IEnumerable<Department>> GetDepartmentForUser(string account)
        {
            return await _shopProvider.GetDepartmentForUser(account);
        }

        ////вернём бесчисленное количество производств
        //public async Task<IEnumerable<Production>> GetAllProductions () {
        //    return await _productionProvider.GetAllProductions ();
        //}

        //public async Task<IEnumerable<ProductionForecast>> GetForecastProductions()
        //{
        //    return await _productionProvider.GetForecastProductions();
        //}

        //public async Task<IEnumerable<BBSSRWK>> GetA020Productions()
        //{
        //    return await _productionProvider.GetA020Productions();
        //}

        //public async Task<IEnumerable<BBSSRWK>> GetA090Productions()
        //{
        //    return await _productionProvider.GetA090Productions();
        //}

        //public async Task<IEnumerable<VehicleAchievement>> GetVehicleAchievement()
        //{
        //    return await _productionProvider.GetVehicleAchievement();
        //}

        //public async Task<IEnumerable<Production>> GetProductionsForUser(string userRu)
        //{
        //    return await _productionProvider.GetProductionsForUser(userRu);
        //}

        //public async Task<IEnumerable<Production>> GetProductionsForShift(int shift)
        //{
        //    return await _productionProvider.GetProductionsForShift(shift);
        //}

        //public async Task<IEnumerable<Shift>> GetShifts()
        //{
        //    return await _productionEssence.GetShifts();
        //}

        //public async Task<IEnumerable<Part>> GetParts()
        //{
        //    return await _productionEssence.GetParts();
        //}
        //public async Task<IEnumerable<Detail>> GetAllDetails()
        //{
        //    return await _productionEssence.GetAllDetails();
        //}



        //public async Task<IEnumerable<Shop>> GetAllShops()
        //{
        //    return await _shopProvider.GetAllShops();
        //}

        //public async Task UpdateDBActualBalance(int actualBalance, string complete, ProductionForecast forecastProductionBoardViewModel)
        //{
        //   await _productionCreator.UpdateActualBalance(actualBalance, complete, forecastProductionBoardViewModel);
        //}

        //public async Task UpdateDBStockStart(int stockStart, ProductionForecast currentEntry)
        //{
        //    await _productionCreator.UpdateStockStart(stockStart,currentEntry);
        //}
        //public async Task UpdateDBForecastStockStart(int stockStart, ProductionForecast currentEntry)
        //{
        //    await _productionCreator.UpdateForecastStockStart(stockStart, currentEntry);
        //}

        //public async Task UpdateDBForecastOrder(int plannedOrder, ProductionForecast currentEntry)
        //{
        //    await _productionCreator.UpdateDBForecastOrder(plannedOrder, currentEntry);
        //}
        //public async Task UpdateDBForecastStockEnd(int forecastStockEnd, ProductionForecast currentEntry)
        //{
        //    await _productionCreator.UpdateDBForecastStockEnd(forecastStockEnd, currentEntry);
        //}
        //public async Task UpdateActualUsage(int actUsage, ProductionForecast currentEntry)
        //{
        //    await _vehicleCreator.UpdateActualUsage(actUsage, currentEntry);
        //}
        //public async Task AddOrder(ProductionForecast currentEntry)
        //{
        //    await _productionCreator.CreateOrderProduction(currentEntry);
        //}
        ////метод добавления производства
        //public async Task AddProduction (Production production) {

        //    //проверка прежде чем добавлять производство, потому что она уже может быть добавлена
        //    Production conflictingProduction = await _productionConflictValidator.GetConflictingProduction (production);

        //    if (conflictingProduction != null) {
        //        //сделать исключение из-за конфликта производств(существующего и входящего)
        //        throw new ProductionConflictException (conflictingProduction, production);
        //    }
        //    await _productionCreator.CreateProduction (production);
        //}

        ////public async Task AddForecastProduction(ProductionForecast production)
        ////{

        ////    //проверка прежде чем добавлять производство, потому что она уже может быть добавлена
        ////    ProductionForecast conflictingForecastProduction = await _productionConflictValidator.GetConflictingForecastProduction(production);

        ////    if (conflictingForecastProduction == null)
        ////    {
        ////        await _productionCreator.CreateForecastProduction(production);
        ////    }

        ////}

        //public async Task AddBBSSRWKItemA020(BBSSRWK production)
        //{
        //    //проверка прежде чем добавлять производство, потому что она уже может быть добавлена
        //    BBSSRWK conflictingProduction = await _productionConflictValidator.GetConflictingForecastProductionA020(production);

        //    if (conflictingProduction == null)
        //    {    
        //        await _productionCreator.CreateForecastProductionA020(production);
        //    }

        //}
        //public async Task AddBBSSRWKItemA090(BBSSRWK production)
        //{
        //    //проверка прежде чем добавлять производство, потому что она уже может быть добавлена
        //    BBSSRWK conflictingProduction = await _productionConflictValidator.GetConflictingForecastProductionA090(production);

        //    if (conflictingProduction == null)
        //    {
        //        await _productionCreator.CreateForecastProductionA090(production);
        //    }

        //}

        //public async Task DeleteProduction(Production production)
        //{
        //    await _productionCreator.DeleteProduction(production);
        //}
        //public async Task DeleteForecastProductions()
        //{
        //    await _productionCreator.DeleteForecastProductions();
        //}
        //public async Task DeleteOrderProductions()
        //{
        //    await _productionCreator.DeleteOrderProductions();
        //}
        //public async Task UpdateProduction(Production production)
        //{
        //    await _productionCreator.UpdateProduction(production);
        //}

        //public async Task AddBBSSItem(BBSS bbss)
        //{
        //    BBSS conflictingBBSS = await _bbssConflictValidator.GetConflictingBBSS(bbss);

        //    if (conflictingBBSS == null)
        //    {
        //        await _bbssCreator.CreateBBSSItem(bbss);
        //    }

        //}

        //public async Task AddBOMItem(BOM bom)
        //{
        //    BOM conflictingBOM = await _bomConflictValidator.GetConflictingBOM(bom);

        //    if (conflictingBOM == null)
        //    {
        //        await _bbssCreator.CreateBOMItem(bom);
        //    }
        //}

        //public async Task AddFAKEItem(FAKE fake)
        //{
        //    FAKE conflictingFAKE = await _fakeConflictValidator.GetConflictingFAKE(fake);

        //    if (conflictingFAKE == null)
        //    {
        //        await _bbssCreator.CreateFAKEItem(fake);
        //    }
        //}
    }
}
