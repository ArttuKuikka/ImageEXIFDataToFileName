#MAKE SURE CURRENT WORKING DIRECTORY IS CORRECT!

import os
from GPSPhoto import gpsphoto
from GPSPhoto.gpsphoto import GPSPhoto
from GPSPhoto.gpsphoto import GPSInfo

def remove_gps_data_from_images():
    for root, dirs, files in os.walk(os.getcwd()):
        for file in files:
            if file.lower().endswith(('.png', '.jpg', '.jpeg')):
                file_path = file
                try:
                    photo = GPSPhoto(file_path)
                    photo.stripData(file)
                except Exception as e:
                    print(f"Skipping {file_path}: {e}")

if __name__ == "__main__":

    remove_gps_data_from_images()
