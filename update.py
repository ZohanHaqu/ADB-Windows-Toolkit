import tkinter as tk
from tkinter import messagebox
import os
import webbrowser
import requests

def download_and_install():
    url = "https://github.com/ZohanHaqu/ADB-Windows-Toolkit/releases/download/1.0/ADBToolkitSetup.msi"
    filename = "ADBToolkitSetup.msi"
    
    try:
        response = requests.get(url, stream=True)
        
        with open(filename, "wb") as file:
            for chunk in response.iter_content(chunk_size=8192):
                file.write(chunk)
        
        os.system(f"msiexec /i {filename} /qb")
    except Exception as e:
        messagebox.showerror("Error", f"Failed to download: {e}")

def open_github():
    webbrowser.open("https://github.com/ZohanHaqu/ADB-Windows-Toolkit")

root = tk.Tk()
root.title("ADB Windows Toolkit Update/Reinstall")
root.geometry("400x250")
root.resizable(False, False)

title_label = tk.Label(root, text="ADB Windows Toolkit Update/Reinstall", font=("Arial", 12, "bold"))
title_label.pack(pady=10)

description = "Click 'Update' to install the latest version.\nClick 'Reinstall' to fix corrupted files."
description_label = tk.Label(root, text=description, wraplength=380, justify="center")
description_label.pack(pady=5)

update_button = tk.Button(root, text="Update", command=download_and_install, width=15)
update_button.pack(pady=5)

reinstall_button = tk.Button(root, text="Reinstall", command=download_and_install, width=15)
reinstall_button.pack(pady=5)

info_button = tk.Button(root, text="More Info", command=open_github, width=15)
info_button.pack(pady=10)

exit_button = tk.Button(root, text="Exit", command=root.quit, width=15)
exit_button.pack(pady=5)

root.mainloop()