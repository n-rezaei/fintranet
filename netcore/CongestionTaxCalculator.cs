using System;
using System.Collections.Generic;
using System.Linq;
using congestion.calculator;
public class CongestionTaxCalculator
{
    /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total congestion tax for that day
         */

    // you can pass TollFeeMetthd out of method
    public int GetTax(Func<DateTime, Vehicle, int> TollFee, Vehicle vehicle, DateTime[] dates, int maximumTaxPerDay)
    {
        var data = dates.GroupBy(x => x.Date).Select(x => new
        {
            date = x.Key,
            intervals = x.ToList()
        });
        var fee = data.Sum(x => GetDayTax(TollFee, vehicle, x.intervals.ToList(), maximumTaxPerDay));
        return fee;
    }

    public int GetDayTax(Func<DateTime, Vehicle, int> TollFee, Vehicle vehicle, List<DateTime> dateTimes, int maximumTaxPerDay)
    {
        var maxFee = dateTimes.Max(x => TollFee(x, vehicle));
        return maxFee > maximumTaxPerDay ? maximumTaxPerDay : maxFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        string vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorcycle.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        // condition was wrong
        var minuteOfDay = date.Hour * 60 + date.Minute;

        if (minuteOfDay >= 6 * 60 && minuteOfDay <= 6 * 60 + 29) return 8;
        else if (minuteOfDay >= 6 * 60 + 30 && minuteOfDay <= 6 * 60 + 59) return 13;
        else if (minuteOfDay >= 7 * 60 && minuteOfDay <= 7 * 60 + 59) return 18;
        else if (minuteOfDay >= 8 * 60 && minuteOfDay <= 8 * 60 + 29) return 13;
        else if (minuteOfDay >= 8 * 60 + 30 && minuteOfDay <= 14 * 60 + 59) return 8;
        else if (minuteOfDay >= 15 * 60 && minuteOfDay <= 15 * 60 + 29) return 13;
        else if (minuteOfDay >= 15 * 60 + 30 && minuteOfDay <= 16 * 60 + 59) return 18;
        else if (minuteOfDay >= 17 * 60 && minuteOfDay <= 17 * 60 + 59) return 13;
        else if (minuteOfDay >= 18 * 60 && minuteOfDay <= 18 * 60 + 29) return 8;
        else return 0;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == 2013)
        {
            if (month == 1 && day == 1 ||
                month == 3 && (day == 28 || day == 29) ||
                month == 4 && (day == 1 || day == 30) ||
                month == 5 && (day == 1 || day == 8 || day == 9) ||
                month == 6 && (day == 5 || day == 6 || day == 21) ||
                month == 7 ||
                month == 11 && day == 1 ||
                month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }
        return false;
    }

    private enum TollFreeVehicles
    {
        Motorcycle = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}