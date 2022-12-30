# Manul

Исходники бота для Discord-а.

## Зависимости

Для работы музыкальной части бота необходимы следующие зависимости:

### Windows
- [**FFmpeg**](http://ffmpeg.zeranoe.com/builds/) и положить в папку с .dll-ками
- [**youtube-dl**](https://rg3.github.io/youtube-dl/download.html) и положить в папку с .dll-ками
- Скачать [**Sodium**](https://discord.foxbot.me/binaries/libsodium/) и [**Opus**](https://discord.foxbot.me/binaries/opus/) и положить в папку с .dll-ками

### Linux
- [**FFmpeg**](https://ffmpeg.org/download.html#build-linux)
- [**youtube-dl**](https://rg3.github.io/youtube-dl/download.html)
```shell
sudo curl -L https://yt-dl.org/downloads/latest/youtube-dl -o /usr/local/bin/youtube-dl
sudo chmod a+rx /usr/local/bin/youtube-dl
```
- Установить [**Sodium**](https://download.libsodium.org/doc/installation/) и [**Opus**](http://opus-codec.org/downloads/)
