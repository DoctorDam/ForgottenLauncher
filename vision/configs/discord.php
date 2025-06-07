<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

/*
    DISCORD SETTINGS
    --------------------------------------------------------
    These settings are used by the launcher crash error report

    This will send the crash error message to your discord server's designated channel webhook

    If you are not sure how to create a webhook url, please follow this guide over here:

    https://www.digitalocean.com/community/tutorials/how-to-use-discord-webhooks-to-get-notifications-for-your-website-status-on-ubuntu-18-04
*/

$discord = [
    'invite_url'        => 'https://discord.gg/2twUE9xU9H',
    'server_id'         => '889616857806549003',
    'bot_token'         => 'MTIzMDQ3MjI5NzE0MDMyNjQ1MA.G0IFsP.dvBBdRmTGiROrXmIAreExIP170ynKsYnS92Bt4',
    'webhookurl'        => 'https://discord.com/api/webhooks/1091665924639170600/nldyS6vgAowqK__qNdrySwZ3nunqweDLmwnHqCIMTrE6lReN7aYv-AF0huStPAK2Pp8c',
    // Embed left border color in HEX, you can generate a code here: https://htmlcolorcodes.com/
    "bordercolor"       => hexdec("FEB300"),
    // Enable or disable mentions in reports
    'mentions_enable'   => false,
    // User ids (enable developer mode in discord then right click on a user "Copy ID")
    'mentions'          =>
    [
        0,
        // etc,
        // etc,
        // etc,
    ],
    /* DISCORD REQUESTS SETTINGS
        max_requests    =   (count)     > number of discord info requests allowed in the interval below
        requests_delay  =   (seconds)   > how many seconds is user blocked after reaching maximum requests
    */
    'max_requests'      => 1,
    'requests_delay'    => 60
];
