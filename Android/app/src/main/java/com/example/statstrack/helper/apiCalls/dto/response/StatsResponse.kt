package com.example.statstrack.helper.apiCalls.dto.response

// Top-level response data class
data class StatsResponse(
    val userStats: UserStats,
    val eduStats: EduStats,
    val expenseStats: ExpenseStats,
    val hobbyStats: HobbyStats,
    val incomeStats: IncomeStats,
    val industryStats: IndustryStats,
    val personalStats: PersonalStats,
    val workingHoursStats: WorkingHoursStats
)

// UserStats data class
data class UserStats(
    val sumTotalUsers: Int,
    val sumMales: Int,
    val sumFemales: Int,
    val sumYoungers: Int,
    val sumAdults: Int,
    val sumOlderAdults: Int
)

// EduStats data class
data class EduStats(
    val sumPrimary: Int,
    val sumSecondary: Int,
    val sumHigher: Int,
    val sumTechnical: Int
)

// ExpenseStats data class
data class ExpenseStats(
    val sumHousingExpenses: Int,
    val sumUtilities: Int,
    val sumTransportationCosts: Int,
    val sumFoodAndGroceries: Int,
    val sumHealthcareExpenses: Int,
    val sumEducationExpenses: Int,
    val sumDebtPayments: Int,
    val sumInsurancePremiums: Int,
    val sumEntertainmentAndLeisure: Int,
    val sumPersonalCareAndGrooming: Int,
    val sumClothingAndApparel: Int,
    val sumSavingsAndInvestments: Int,
    val sumTaxes: Int,
    val sumChildcareAndEducation: Int,
    val sumHomeAndGarden: Int,
    val sumSubscriptionsAndMemberships: Int,
    val sumCharitableDonations: Int,
    val sumTravelAndVacation: Int,
    val sumLegalAndFinancialServices: Int,
    val sumEmergencyFundAndContingencies: Int,
    val sumOther: Int
)

// HobbyStats data class
data class HobbyStats(
    val sumReading: Int,
    val sumGardening: Int,
    val sumCookingAndBaking: Int,
    val sumPaintingAndDrawing: Int,
    val sumSportsAndPhysicalActivities: Int,
    val sumPhotography: Int,
    val sumPlayingMusicalInstruments: Int,
    val sumTraveling: Int,
    val sumCrafting: Int,
    val sumFishing: Int,
    val sumModelBuilding: Int,
    val sumCollecting: Int,
    val sumHikingAndCamping: Int,
    val sumDIYHomeImprovement: Int,
    val sumBirdWatching: Int,
    val sumCycling: Int,
    val sumMeditationAndMindfulness: Int,
    val sumDancing: Int,
    val sumVolunteering: Int,
    val sumBoardGamesAndPuzzles: Int,
    val sumWriting: Int,
    val sumFitnessAndExercise: Int,
    val sumComicsAndGraphicNovels: Int,
    val sumSculpting: Int,
    val sumAstrologyAndAstronomy: Int,
    val sumSewingAndQuilting: Int,
    val sumArchery: Int,
    val sumGenealogy: Int,
    val sumMetalDetecting: Int,
    val sumVintageAndClassicCars: Int,
    val sumOther: Int
)

// IncomeStats data class
data class IncomeStats(
    val sumPovertyLine: Int,
    val sumLowIncome: Int,
    val sumLowerMiddleIncome: Int,
    val sumMedianIncome: Int,
    val sumUpperMiddleIncome: Int,
    val sumHighIncome: Int,
    val sumVeryHighIncome: Int
)

// IndustryStats data class
data class IndustryStats(
    val sumInformationTechnology: Int,
    val sumHealthcare: Int,
    val sumFinanceAndBanking: Int,
    val sumEducation: Int,
    val sumEngineering: Int,
    val sumManufacturingAndProduction: Int,
    val sumRetail: Int,
    val sumHospitalityAndTourism: Int,
    val sumMarketingAndAdvertising: Int,
    val sumGovernmentAndPublicAdministration: Int,
    val sumLegal: Int,
    val sumAgriculture: Int,
    val sumMediaAndJournalism: Int,
    val sumArtAndDesign: Int,
    val sumNonprofitAndSocialServices: Int,
    val sumEnvironmentalAndSustainability: Int,
    val sumConstructionAndSkilledTrades: Int,
    val sumAutomotive: Int,
    val sumTransportationAndLogistics: Int,
    val sumSportsAndFitness: Int,
    val sumOnlineBusinesses: Int,
    val sumPharmaceuticalsAndHealthcareResearch: Int,
    val sumEntertainmentAndArts: Int,
    val sumTechStartupsAndEntrepreneurship: Int,
    val sumRealEstateAndPropertyManagement: Int,
    val sumFoodAndBeverage: Int,
    val sumAviationAndAerospace: Int,
    val sumRenewableEnergyAndGreenTechnology: Int,
    val sumFitnessAndWellness: Int,
    val sumEmergencyServicesAndPublicSafety: Int,
    val sumStudent: Int,
    val sumRetired: Int
)

// PersonalStats data class
data class PersonalStats(
    val sumStayHome: Int,
    val sumGoOut: Int,
    val sumMarried: Int,
    val sumSingle: Int
)

// WorkingHoursStats data class
data class WorkingHoursStats(
    val sumFullTimeEmployment: Int,
    val sumPartTimeEmployment: Int,
    val sumOvertimeHours: Int,
    val sumShiftWork: Int,
    val sumFlexibleHours: Int,
    val sumSeasonalWork: Int,
    val sumFreelancingWork: Int,
    val sumContractWork: Int,
    val sumRemoteWork: Int,
    val sumNightShifts: Int,
    val sumWeekendWork: Int,
    val sumUnemployed: Int
)
