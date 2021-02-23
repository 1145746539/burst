using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OEE.bean
{
    class TestParament
    {
    }

    /// <summary>
    /// 单位的枚举
    /// </summary>
    enum Unit : int
    {
        /// <summary>
        /// 每秒X立方厘米
        /// </summary>
        立方厘米每秒 = 0,

        /// <summary>
        /// 每分钟x立方厘米
        /// </summary>
        CM3_PerMinute = 1,


        /// <summary>
        /// 校准气压
        /// </summary>
        Calibrated_Pa = 4,

        /// <summary>
        /// 帕斯卡
        /// </summary>
        Pa = 6,

        /// <summary>
        /// 每秒x帕斯卡
        /// </summary>
        帕每秒 = 8,

        秒 = 10,

        /// <summary>
        /// 
        /// </summary>
        Bar = 11,

        /// <summary>
        /// 迷你
        /// </summary>
        MiniBar = 14,

        标准毫升每分钟 = 84

    }

    /// <summary>
    /// 测试过程的枚举
    /// </summary>
    enum Step
    {
        预充气 = 0,
        预放气 = 1,
        密封组件充气 = 2,
        密封组件稳压 = 3,
        充气 = 4,
        稳压 = 5,
        测试 = 6,
        泄气 = 7,
        升压 = 10,
        保持 = 11,
        空闲 = 65535
    }

    /// <summary>
    /// 测试状态的枚举
    /// </summary>
    enum TestStatus
    {
        通过 = 0,
        Fail_Part_Maximum_Flow = 1,
        Fail_Part_Minimum = 2,
        报警 = 3,
        Pressure_Error = 4,
        Recoverable_Part = 6,
        CAL_Error_Or_Drift = 7,
        Calibration_Check_Error = 8,
        ATR_Error_Or_Drift = 9
    }

    enum AlarmCode
    {
        No_alarm = 0,
        test_pressure_too_high = 1,
        test_pressure_too_small = 2,
        Large_leak_on_TEST = 3,
        Large_leak_on_REF = 4,
        Sensor_out_of_order = 7,
        ATR_error_ = 8,
        ATR_drift_ = 9,
        CAL_error_ = 10,
        Volume_too_small = 11,
        Volume_too_large = 12,
        Equalization_valve_switching_error = 14,
        Pressure_toohigh = 43,
        Pressure_toolow = 44,
        Piezo_sensor_out_of_order = 45,
        Dump_error = 46,
        CAL_drift_error = 47,
        Calibration_check_error = 48,
        Leak_in_calibration_check_too_high = 49,
        Leak_in_calibration_check_too_low = 50,
        Sealed_component_learning_error = 51,
        Piezo_sensor_2_out_of_order = 64,
        Pressure_Piezo_2_too_high = 65,
        Pressure_Piezo_2_too_low = 66,
        Pressure_Piezo_2_switched_alarm = 68,
        Pressure_Piezo_2_switch = 69,
        Learning_Electrical_Regulator_Default = 72,
        Pressure_too_low = 75
    }
}
