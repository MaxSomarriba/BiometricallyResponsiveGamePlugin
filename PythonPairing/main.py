# Bluetooth scanner
# Prints the name and address of every nearby Bluetooth LE device

# E6:7F:A6:74:4F:F0: HW706-0040796
import asyncio
import numpy as np
import matplotlib.pyplot as plt
from bleak import BleakClient
from collections import deque
from matplotlib.animation import FuncAnimation
import keyboard  # To detect key combinations
import pandas as pd  # To export data to CSV
from datetime import datetime  # To track timestamps

# Replace with your device's Bluetooth address
device_address = "E6:7F:A6:74:4F:F0"

# Heart Rate Measurement UUID
HRM_UUID = "00002a37-0000-1000-8000-00805f9b34fb"

# Set up data storage
data_points = deque(maxlen=300)
timestamps = deque(maxlen=300)  # Store timestamps for each heart rate reading

# Flag to control when to stop the loop
stop_flag = False

# Function to parse heart rate data
def parse_heart_rate(data):
    flags = data[0]
    heart_rate_format = flags & 0x01
    
    if heart_rate_format == 0:
        heart_rate = data[1]
    else:
        heart_rate = int.from_bytes(data[1:3], byteorder='little')
    
    return heart_rate

# Function to handle notifications
def notification_handler(sender, data):
    heart_rate = parse_heart_rate(data)
    print(f"Heart rate: {heart_rate} bpm")
    data_points.append(heart_rate)
    timestamps.append(datetime.now())  # Record the current timestamp

# Async function to connect to the device and start notifications
async def run():
    global stop_flag
    async with BleakClient(device_address) as client:
        print(f"Connected: {client.is_connected}")
        await client.start_notify(HRM_UUID, notification_handler)

        while not stop_flag:
            await asyncio.sleep(1)

        await client.stop_notify(HRM_UUID)

# Function to update the graph
def update_graph(frame):
    if data_points:
        plt.cla()  # Clear the previous frame
        plt.plot(np.arange(len(data_points)), data_points, label="Heart Rate (bpm)")
        plt.ylim(40, 180)  # Set y-limits for heart rate range
        plt.title("Real-time Heart Rate Monitor")
        plt.xlabel("Time (s)")
        plt.ylabel("Heart Rate (bpm)")
        plt.legend(loc="upper right")
        plt.tight_layout()

# Function to listen for key combination (Ctrl+Q) to stop the script
def stop_script_on_key_combination():
    global stop_flag
    while not stop_flag:
        if keyboard.is_pressed('ctrl+q'):  # Key combination to stop the script
            stop_flag = True
            print("Ctrl+Q pressed, stopping the script...")

# Function to export data to CSV
def export_data_to_csv(filename="heart_rate_data.csv"):
    if data_points:
        # Create a pandas DataFrame with the heart rate data and timestamps
        df = pd.DataFrame({
            'Timestamp': list(timestamps),
            'Heart Rate (bpm)': list(data_points)
        })
        # Export the DataFrame to a CSV file
        df.to_csv(filename, index=False)
        print(f"Data exported to {filename}")

# Set up matplotlib plot
plt.ion()  # Enable interactive mode
fig, ax = plt.subplots()
ani = FuncAnimation(fig, update_graph, interval=1000)  # Update every second

# Create a thread to handle stopping the script on key combination
async def main():
    # Run the key listener and Bluetooth client concurrently
    key_listener = asyncio.get_event_loop().run_in_executor(None, stop_script_on_key_combination)
    await run()
    await key_listener

# Run the event loop in the background
loop = asyncio.get_event_loop()
loop.run_in_executor(None, loop.run_until_complete, main())

# Show the plot (non-blocking)
plt.show(block=False)  # Use non-blocking mode

# Wait for the event loop to finish
while not stop_flag:
    plt.pause(0.1)  # Allow the plot to update without freezing the script

# After the script is stopped, export the data to a CSV file
export_data_to_csv("heart_rate_data.csv")
