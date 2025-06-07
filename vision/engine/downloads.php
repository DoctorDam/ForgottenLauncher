<?php
/*

█▄█ ███ ███ ███ ███ █┼┼█ ┼┼ ███ ███ ███
███ ┼█┼ █▄▄ ┼█┼ █┼█ ██▄█ ┼┼ █▄█ █▄█ ┼█┼
┼█┼ ▄█▄ ▄▄█ ▄█▄ █▄█ █┼██ ┼┼ █┼█ █┼┼ ▄█▄

Copyrights @ cybermist2 2024-present

API built by cybermist2@gmail.com

*/

namespace vISION;

class DOWNLOADS
{
    protected $config;

    /**
     * Constructor for initializing the object with configuration and database parameters.
     *
     * @param mixed $config   The configuration data.
     */
    public function __construct($config)
    {
        $this->config = $config;
    }

    /*
    * Returns game files list as json
    *
    * @return json
    */
    public function ListGameFiles()
    {
        $GameFiles = [];

        $files = $this->GetGameFiles();

        $protocol = isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? 'https://' : 'http://';
        $domain = $protocol . $_SERVER['HTTP_HOST'];

        foreach ($files as $file)
        {
            $fileInfo = new \stdClass();
            $fileInfo->Name = basename($file); // Get the file name
            $fileInfo->Size = $this->GetRealFileSize($file); // Get the file size
            $fileInfo->Timestamp = filemtime($file); // Get the file last modified time

            // works with php 7 and 8
            $fileInfo->IsBase = strpos($file, "base-client/") !== false || strpos($file, "base-client\\") !== false;

            // works with php 7 and 8
            $fileInfo->IsHD = strpos($file, "base-hd/") !== false || strpos($file, "base-hd\\") !== false;

            // works with php 7 and 8
            $fileInfo->IsUpdate = strpos($file, "push-updates/") !== false || strpos($file, "push-updates\\") !== false;

            // works with php 7 and 8
            $fileInfo->IsProgramData = strpos($file, "program-data/") !== false || strpos($file, "program-data\\") !== false;

            $fileInfo->TargetPath = str_replace(
            [
                'downloads\\game',
                'downloads/game',
                'base-client\\',
                'base-client/',
                'base-hd\\',
                'base-hd/',
                'push-updates\\',
                'push-updates/',
                'program-data\\',
                'program-data/'
            ], '', $file);

            $fileInfo->Url = $domain.$this->GetApiPath()."/".str_replace('\\', '/', $file);

            $GameFiles[] = $fileInfo;
        }

        return json_encode($GameFiles, JSON_PRETTY_PRINT);
    }

    /*
    * Returns launcher update files list as json
    *
    * @return json
    */
    public function ListLauncherUpdateFiles()
    {
        $LauncherFiles = [];

        $files = $this->GetLauncherFiles();

        $protocol = isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? 'https://' : 'http://';
        $domain = $protocol . $_SERVER['HTTP_HOST'];

        foreach ($files as $file)
        {
            $fileInfo = new \stdClass();
            $fileInfo->Name = basename($file); // Get the file name
            $fileInfo->Size = $this->GetRealFileSize($file); // Get the file size
            $fileInfo->Timestamp = filemtime($file); // Get the file last modified time
            $fileInfo->MD5_Hash = md5_file($file);

            $fileInfo->TargetPath = str_replace(
                [
                    'downloads\\launcher',
                    'downloads/launcher',
                ], '', $file);

            $fileInfo->Url = $domain.$this->GetApiPath()."/".str_replace('\\', '/', $file);

            $LauncherFiles[] = $fileInfo;
        }

        return json_encode($LauncherFiles, JSON_PRETTY_PRINT);
    }

    /*
    * Returns addons files list as json
    *
    * @return json
    */
    public function ListAddons()
    {
        $addons = $this->GetAddonsList();
        $addons_list = [];

        foreach($addons as $addon)
        {
            $addonInfo = new \stdClass();
            $addonInfo->Name = $addon;
            $addonInfo->Description = $this->GetAddonDescription($addon);
            $addonInfo->PictureUrl = $this->GetAddonPictureUrl($addon);
            $addonInfo->TotalSize = $this->GetAddonSize($addon);
            $addonInfo->Files = $this->GetAddonFiles($addon);

            $addons_list[] = $addonInfo;
        }

        return json_encode($addons_list, JSON_PRETTY_PRINT);
    }

    /*
    * Returns addon's description
    *
    * @param string $addon_name
    * @return array
    */
    private function GetAddonDescription($addon_name)
    {
        $description = "";

        $files = scandir("downloads/addons/".$addon_name);
        foreach ($files as $file)
        {
            if ($file !== '.' && $file !== '..' && strpos($file, "addon_description.txt") !== false)
            {
                $file_path = realpath($_SERVER["DOCUMENT_ROOT"]).$this->GetApiPath()."/downloads/addons/".$addon_name."/".$file;
                $description_file = fopen($file_path, "r") or die("Unable to open file!");
                $description = fread($description_file, filesize($file_path));
                fclose($description_file);
            }
        }

        return $description;
    }

    /*
    * Returns addon's picture url
    *
    * @param string $addon_name
    * @return array
    */
    private function GetAddonPictureUrl($addon_name)
    {
        $protocol = isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? 'https://' : 'http://';
        $domain = $protocol . $_SERVER['HTTP_HOST'];
        $picture_name = "";

        $files = scandir("downloads/addons/".$addon_name);
        foreach ($files as $file)
        {
            if ($file !== '.' && $file !== '..' && strpos($file, "addon_picture") !== false)
            {
                $picture_name = $file;
            }
        }

        return $domain.$this->GetApiPath()."/downloads/addons/".$addon_name."/".$picture_name;
    }

    /*
    * Returns addon's files array
    *
    * @return array
    */
    private function GetAddonFiles($addon_name) : array
    {
        $iterator = new \RecursiveIteratorIterator
        (
            new \RecursiveDirectoryIterator("downloads/addons/".$addon_name, 
                \RecursiveDirectoryIterator::SKIP_DOTS),
                \RecursiveIteratorIterator::SELF_FIRST
        );

        $protocol = isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? 'https://' : 'http://';
        $domain = $protocol . $_SERVER['HTTP_HOST'];

        $files = [];

        foreach ($iterator as $file)
        {
            if ($file->isFile() && !str_contains($file, "addon_picture") && !str_contains($file, "addon_description"))
            {
                $local_target = str_replace(
                    [
                        'downloads\\addons\\'.$addon_name,
                        'downloads/addons/'.$addon_name,
                    ], '', $file);

                $local_target = "interface/addons".$local_target;
                $local_target = str_replace('/', '\\', $local_target);

                $files[] = 
                [
                    'size' => filesize($file),
                    'url' => $domain.$this->GetApiPath()."/".str_replace('\\', '/', $file),
                    'target' => $local_target,
                ];
            }
        }

        return $files;
    }

    /*
    * Returns formatted bytes
    *
    * @return string
    */
    private function formatBytes($size)
    { 
        $units = array('B', 'KB', 'MB', 'GB', 'TB');

        for ($i = 0; $size >= 1024 && $i < count($units) - 1; $i++)
        {
            $size /= 1024;
        }

        if ($i === 1)
        {
            return round($size) . ' ' . $units[$i];
        }
        else
        {
            return round($size, 2) . ' ' . $units[$i];
        }
    } 

    /*
    * Returns addon's total size as string
    *
    * @return string
    */
    private function GetAddonSize($addon_name) : string
    {
        $iterator = new \RecursiveIteratorIterator
        (
            new \RecursiveDirectoryIterator("downloads/addons/".$addon_name, 
                \RecursiveDirectoryIterator::SKIP_DOTS),
                \RecursiveIteratorIterator::SELF_FIRST
        );

        $total_size = 0;

        foreach ($iterator as $file)
        {
            if ($file->isFile() && !str_contains($file, "addon_picture"))
            {
                $total_size += filesize($file);
            }
        }

        return $total_size;
    }

    /*
    * Returns game files array
    *
    * @return array
    */
    private function GetGameFiles() : array
    {
        $iterator = new \RecursiveIteratorIterator
        (
            new \RecursiveDirectoryIterator("downloads/game", \RecursiveDirectoryIterator::SKIP_DOTS),
                \RecursiveIteratorIterator::SELF_FIRST
        );

        $files = [];

        foreach ($iterator as $file)
        {
            if ($file->isFile())
            {
                $files[] = $file->getPathname();
            }
        }

        return $files;
    }

    /*
    * Returns launcher files array
    *
    * @return array
    */
    private function GetLauncherFiles() : array
    {
        $iterator = new \RecursiveIteratorIterator
        (
            new \RecursiveDirectoryIterator("downloads/launcher", \RecursiveDirectoryIterator::SKIP_DOTS),
                \RecursiveIteratorIterator::SELF_FIRST
        );

        $files = [];

        foreach ($iterator as $file)
        {
            if ($file->isFile())
            {
                $files[] = $file->getPathname();
            }
        }

        return $files;
    }

    /*
     * Returns addons list as array
     *
     * @return array
     */
    private function GetAddonsList(): array
    {
        $addonDirectory = "downloads/addons";

        if (!is_dir($addonDirectory))
        {
            return json_encode([], JSON_PRETTY_PRINT);
        }

        $folders = [];

        $iterator = new \DirectoryIterator($addonDirectory);

        foreach ($iterator as $file)
        {
            if ($file->isDir() && !$file->isDot())
            {
                $folders[] = $file->getFilename();
            }
        }

        return $folders;
    }

    /*
    * Returns real file size, handling also bigger file size
    *
    * @param string $pfn - represents parent folder name
    * @return big or small integger
    */
    private function GetRealFileSize(string $path)
    {
        if (!file_exists($path))
            return false;

        $size = filesize($path);

        if (!($file = fopen($path, 'rb')))
            return false;

        if ($size >= 0)
        {
            // Check if it really is a small file (< 2 GB)
            if (fseek($file, 0, SEEK_END) === 0)
            {
                // It really is a small file
                fclose($file);
                return $size;
            }
        }

        // Quickly jump the first 2 GB with fseek. After that fseek is not working on 32 bit php (it uses int internally)
        $size = PHP_INT_MAX - 1;
        if (fseek($file, PHP_INT_MAX - 1) !== 0)
        {
            fclose($file);
            return false;
        }

        $length = 1024 * 1024;
        while (!feof($file))
        {
            // Read the file until end
            $read = fread($file, $length);
            $size = bcadd($size, $length);
        }

        $size = bcsub($size, $length);
        $size = bcadd($size, strlen($read));

        fclose($file);
        return $size;
    }

    /*
    * Returns api path starting from root folder
    * Root folder name excluded
    * File Name excluded
    *
    */
    private function GetApiPath()
    {
        $uri_parts = explode('/', $_SERVER['REQUEST_URI']);
        array_pop($uri_parts); // Remove the last element (the file name)
        $url = implode('/', $uri_parts);
        return $url;
    }
}