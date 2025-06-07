<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;

class MAILER
{
    protected $config;

    public function __construct($config)
    {
        $this->config = $config;
    }

    function SendPasswordResetCode($receiverEmail, $accountName, $recovery_code)
    {
        global $config;
        $mail = new PHPMailer(true);

        // Server settings
        $mail->SMTPDebug = SMTP::DEBUG_OFF;                 // Enable verbose debug output
        $mail->isSMTP();                                    // Send using SMTP
        $mail->Host       = $config['smtp']['host'];        // Set the SMTP server to send through
        $mail->SMTPAuth   = true;                           // Enable SMTP authentication
        $mail->Username   = $config['smtp']['username'];    // SMTP username
        $mail->Password   = $config['smtp']['password'];    // SMTP password
        $mail->SMTPSecure = PHPMailer::ENCRYPTION_SMTPS;    // Enable implicit TLS encryption
        $mail->Port       = $config['smtp']['port'];        // TCP port to connect to; use 587 if you have set `SMTPSecure = PHPMailer::ENCRYPTION_STARTTLS`

        // Recipients
        $mail->setFrom($config['smtp']['username'], $config['smtp']['from_name']);
        $mail->addAddress($receiverEmail, $accountName);
        $mail->addReplyTo($config['smtp']['reply_to_email'], $config['smtp']['reply_to_name']);

        //Content
        $mail->isHTML(true);    //Set email format to HTML
        $mail->Subject = 'Password recovery code for '.$accountName;

        $mail->Body = 'Greetings <b>'.$accountName.'</b>,<br/><br/>Your password recovery code is: <b>'.$recovery_code.'</b>
        <br/><br/>The code is valid for '.$config['recovery_code']['valid_time'].' seconds!
        <br/><br/>Good luck!';

        $mail->AltBody = 'Greetings '.$accountName.', Your password reset code is: '.$recovery_code.' valid for '.$config['recovery_code']['valid_time'].' seconds!';

        $mail->send();
    }
}