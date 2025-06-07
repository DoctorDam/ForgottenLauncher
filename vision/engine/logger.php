<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class LOGGER
{
    function AppendMysqlError($message)
    {
        $logDirectory = './logs';
        $logFilePath = $logDirectory . '/errors.log';
    
        if (!is_dir($logDirectory))
        {
            mkdir($logDirectory, 0755, true);
        }
    
        $timestamp = date('Y-m-d H:i:s');
    
        $logMessage = "[$timestamp-MYSQL ERROR]: $message\n";
    
        error_log($logMessage, 3, $logFilePath);
    }

    function AppendOtherError($message)
    {
        $logDirectory = './logs';
        $logFilePath = $logDirectory . '/errors.log';
    
        if (!is_dir($logDirectory))
        {
            mkdir($logDirectory, 0755, true);
        }
    
        $timestamp = date('Y-m-d H:i:s');
    
        $logMessage = "[$timestamp-OTHER ERROR]: $message\n";
    
        error_log($logMessage, 3, $logFilePath);
    }
}
