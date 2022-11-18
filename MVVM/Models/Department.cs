using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vacation_Portal.MVVM.Models
{
    public class Department {
        private readonly VacationSummary _vacationSummary;

        public string Name { get; set; }

        public Department (string name, VacationSummary vacationSummary = null) {
            Name = name;
            _vacationSummary = vacationSummary;
        }

        public async Task<IEnumerable<Settings>> GetSettingsUI(string user)
        {
            return await _vacationSummary.GetSettingsUI(user);
        }

        //public async Task<IEnumerable<Production>> GetAllProductions () {
        //    return await _productionSummary.GetAllProductions ();
        //}

        //public async Task<IEnumerable<ProductionForecast>> GetForecastProductions()
        //{
        //    return await _productionSummary.GetForecastProductions();
        //}

        //public async Task<IEnumerable<BBSSRWK>> GetA020Productions()
        //{
        //    return await _productionSummary.GetA020Productions();
        //}

        //public async Task<IEnumerable<BBSSRWK>> GetA090Productions()
        //{
        //    return await _productionSummary.GetA090Productions();
        //}

        //public async Task<IEnumerable<VehicleAchievement>> GetVehicleAchievement()
        //{
        //    return await _productionSummary.GetVehicleAchievement();
        //}

        //public async Task<IEnumerable<Production>> GetProductionsForShift(int shift)
        //{
        //    return await _productionSummary.GetProductionsForShift(shift);
        //}


        //public async Task<IEnumerable<Shift>> GetShifts()
        //{
        //    return await _productionSummary.GetShifts();
        //}

        //public async Task<IEnumerable<Shop>> GetAllShops()
        //{
        //    return await _productionSummary.GetAllShops();
        //}

        //public async Task<IEnumerable<Part>> GetParts()
        //{
        //    return await _productionSummary.GetParts();
        //}

        //public async Task<IEnumerable<Detail>> GetAllDetails()
        //{
        //    return await _productionSummary.GetAllDetails();
        //}


        //public async Task AddProduction (Production production) {
        //    await _productionSummary.AddProduction (production);
        //}
        //public async Task UpdateProduction(Production production)
        //{
        //    await _productionSummary.UpdateProduction(production);
        //}
        //public async Task DeleteProduction(Production production)
        //{
        //    await _productionSummary.DeleteProduction(production);
        //}

        //public async Task AddBBSSRWKItemA020(BBSSRWK production) {
        //    await _productionSummary.AddBBSSRWKItemA020(production);
        //}
        //public async Task AddBBSSRWKItemA090(BBSSRWK production)
        //{
        //    await _productionSummary.AddBBSSRWKItemA090(production);
        //}
        //public async Task AddBBSSItem(BBSS bbss)
        //{
        //    await _productionSummary.AddBBSSItem(bbss);
        //}

        //public async Task AddBOMItem(BOM bom)
        //{
        //    await _productionSummary.AddBOMItem(bom);
        //}
        //public async Task AddFAKEItem(FAKE fake)
        //{
        //    await _productionSummary.AddFAKEItem(fake);
        //}

        //public async Task DeleteOrderProductions()
        //{
        //    await _productionSummary.DeleteOrderProductions();
        //}
        //public async Task AddOrder(ProductionForecast currentEntry)
        //{
        //    await _productionSummary.AddOrder(currentEntry);
        //}

        //public async Task UpdateActualUsage(int actUsage, ProductionForecast currentEntry)
        //{
        //    await _productionSummary.UpdateActualUsage(actUsage, currentEntry);
        //}
        //public async Task UpdateDBActualBalance(int actualBalance, string complete, ProductionForecast forecastProductionBoardViewModel)
        //{
        //    await _productionSummary.UpdateDBActualBalance(actualBalance, complete,forecastProductionBoardViewModel);
        //}
        //public async Task UpdateDBStockStart(int stockStart, ProductionForecast currentEntry)
        //{
        //    await _productionSummary.UpdateDBStockStart(stockStart,currentEntry);
        //}

        //public async Task DeleteForecastProductions()
        //{
        //    await _productionSummary.DeleteForecastProductions();
        //}
        //public async Task UpdateDBForecastStockStart(int stockStart, ProductionForecast currentEntry)
        //{
        //    await _productionSummary.UpdateDBForecastStockStart(stockStart, currentEntry);
        //}
        //public async Task UpdateDBForecastOrder(int plannedOrder, ProductionForecast currentEntry)
        //{
        //    await _productionSummary.UpdateDBForecastOrder(plannedOrder, currentEntry);
        //}
        //public async Task UpdateDBForecastStockEnd(int forecastStockEnd, ProductionForecast currentEntry)
        //{
        //    await _productionSummary.UpdateDBForecastStockEnd(forecastStockEnd, currentEntry);
        //}
    }
}
