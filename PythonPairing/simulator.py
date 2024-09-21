import os
import pandas as pd
import random
import time

# File path for the CSV
csv_file_path = "heartbeat_data.csv"

# Delete the CSV file if it exists
if os.path.exists(csv_file_path):
    os.remove(csv_file_path)

# Function to simulate heartbeat data
def simulate_heartbeat():
    while True:
        # Simulate a heart rate value
        heart_rate = random.randint(60, 180)
        # Create a DataFrame with the new heart rate
        df = pd.DataFrame({"Timestamp": [pd.Timestamp.now()], "Heart Rate (bpm)": [heart_rate]})
        
        # Append the new data to the CSV file
        df.to_csv(csv_file_path, mode='a', header=not pd.io.common.file_exists(csv_file_path), index=False)
        
        print(f"Logged heart rate: {heart_rate} bpm")
        time.sleep(1)  # Sleep for 1 second before logging again

if __name__ == "__main__":
    simulate_heartbeat()
