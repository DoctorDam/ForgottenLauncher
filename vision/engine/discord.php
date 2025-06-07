<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class DISCORD_LIB
{
    protected $config;
    protected $discord;
    protected $database;

    public function __construct($config, $discord, $database)
    {
        $this->config = $config;
        $this->discord = $discord;
        $this->database = $database;
    }
    
    private function LAUNCHER()
    {
        return new LAUNCHER($this->config, $this->database);
    }

    public function GetServerInfo()
    {
        $file_cache_path = "./cache/discord_info_cache.json";
        $contents = null;

        if (file_exists($file_cache_path))
        {
            $contents = file_get_contents($file_cache_path);
        }
        else
        {
            $this->Update_Cache_Info_File();
            $contents = file_get_contents($file_cache_path);
            return json_encode(json_decode($contents, true), JSON_PRETTY_PRINT);
        }

        if ($this->LAUNCHER()->Has_Rate_Limiter_Cooldown(RATE_LIMITER_DISCORD_INFO, "0"))
        {
            if ($this->is_json_contents($contents))
            {
                return json_encode(json_decode($contents, true), JSON_PRETTY_PRINT);
            }
            else
            {
                $this->Update_Cache_Info_File(); // update file even if there is a rate limiter because the file is invalid
                $this->GetServerInfo();
            }
        }
        else
        {
            $this->Update_Cache_Info_File();
            $this->LAUNCHER()->Update_Rate_Limiter_Attempts(RATE_LIMITER_DISCORD_INFO, "0");
            sleep(1);
            $contents = file_get_contents($file_cache_path);
            return json_encode(json_decode($contents, true), JSON_PRETTY_PRINT);
        }
    }

    private function Update_Cache_Info_File()
    {
        if (!$this->folder_exist('./cache')) 
        {
            mkdir("./cache");
        }
        
        file_put_contents("./cache/discord_info_cache.json", $this->GetServerInfoJSON());
    }

    private function is_json_contents($string) 
    {
        json_decode($string);
        return json_last_error() === JSON_ERROR_NONE;
    }

    private function folder_exist($folder)
    {
        $path = realpath($folder);

        return ($path !== false AND is_dir($path)) ? $path : false;
    }

    private function GetServerInfoJSON()
    {
        $serverId = $this->discord['server_id'];

        // The URL of the JSON data you want to fetch
        $jsonUrl = 'https://discordapp.com/api/guilds/' . $serverId . '/widget.json';

        // Fetch the JSON data from the URL using cURL
        $ch = curl_init($jsonUrl);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        $jsonData = curl_exec($ch);

        // Check for cURL errors
        if ($jsonData === false) {
            die('Failed to fetch JSON data from the URL: ' . curl_error($ch));
        }

        // Close the cURL session
        curl_close($ch);

        // Parse the JSON data
        $decodedData = json_decode($jsonData);

        // Check if JSON decoding was successful
        if ($decodedData === null) {
            die('Failed to decode JSON data.');
        }

        return json_encode($decodedData, JSON_PRETTY_PRINT);
    }

    public function SendReport($reportBy, $message)
    {
        //=======================================================================================================
        // Compose message. You can use Markdown
        // Message Formatting -- https://discordapp.com/developers/docs/reference#message-formatting
        //========================================================================================================

        $timestamp = date("c", strtotime("now"));

        $mentions = "";

        if ($this->discord['mentions_enable'])
        {
            $mentions = "Automatically mentioned users:";
            foreach ($this->discord['mentions'] as $mention)
            {
                $mentions .= " <@".$mention.">";
            }
        }

        $json_data = json_encode([
            // Message
            "content" => $mentions,

            // Username
            // "username" => "",

            // Avatar URL.
            // Uncoment to replace image set in webhook
            //"avatar_url" => "",

            // Text-to-speech
            "tts" => false,

            // File upload
            // "file" => "",

            // Embeds Array
            "embeds" => [
                [
                    // Embed Title
                    "title" => "Oracle Launcher v2",

                    // Embed Type
                    "type" => "rich",

                    // Embed Description
                    "description" => ":warning: Error Handler",

                    // URL of title link
                    // "url" => "",

                    // Timestamp of embed must be formatted as ISO8601
                    "timestamp" => $timestamp,

                    // Embed left border color in HEX
                    "color" => $this->discord['bordercolor'],

                    // Footer
                    "footer" => [
                        "text" => "User who reported: ".$reportBy,
                        "icon_url" => "https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/avatars/79/79c1502a504b9c04034d8df7469cdf4466518ced_full.jpg"
                    ],

                    // Image to send
                    // "image" => [
                        // "url" => ""
                    // ],

                    // Thumbnail
                    // "thumbnail" => [
                    // "url" => ""
                    // ],

                    // Author
                    // "author" => [
                        // "name" => $reportBy,
                        // "url" => ""
                    // ],

                    // Additional Fields array
                    "fields" => [
                        // Field 1
                        [
                            "name" => "Message:",
                            "value" => $message,
                            "inline" => false
                        ],
                        // Etc..
                    ]
                ]
            ]

        ], JSON_UNESCAPED_SLASHES | JSON_UNESCAPED_UNICODE );


        $ch = curl_init( $this->discord['webhookurl'] );
        curl_setopt( $ch, CURLOPT_HTTPHEADER, array('Content-type: application/json'));
        curl_setopt( $ch, CURLOPT_POST, 1);
        curl_setopt( $ch, CURLOPT_POSTFIELDS, $json_data);
        curl_setopt( $ch, CURLOPT_FOLLOWLOCATION, 1);
        curl_setopt( $ch, CURLOPT_HEADER, 0);
        curl_setopt( $ch, CURLOPT_RETURNTRANSFER, 1);

        $response = curl_exec( $ch );
        // If you need to debug, or find out why you can't send message uncomment line below, and execute script.
        // echo $response;
        curl_close( $ch );
    }
}