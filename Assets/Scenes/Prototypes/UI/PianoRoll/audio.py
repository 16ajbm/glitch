import librosa
import librosa.display
import numpy as np
import matplotlib.pyplot as plt
from scipy.ndimage import gaussian_filter1d

filename = 'One Man Symphony - Collateral Damage (Free) - 15 CyberRace Theme 02.mp3'
y, sr = librosa.load(filename, sr=1000)

time = np.linspace(0, len(y)/sr, num=len(y))

#plt.plot(time[0:3000], np.abs(y[0:3000]), label='Waveform', linewidth=1)
#plt.xlabel('Time [s]')
#plt.ylabel('Amplitude')
#plt.title('Waveform')
#plt.legend()
#plt.grid()
#plt.show()

window_size = 100
moving_averages = np.convolve(np.abs(y), np.ones(window_size)/window_size, mode='same')

#plt.plot(time[0:3000], moving_averages[0:3000], label='Waveform', linewidth=1)
#plt.xlabel('Time [s]')
#plt.ylabel('Amplitude')
#plt.title('Waveform')
#plt.legend()
#plt.grid()
#plt.show()

#plt.plot(time[0:10000], (moving_averages[0:10000])/np.max(moving_averages), label='Waveform', linewidth=1)
#plt.xlabel('Time [s]')
#plt.ylabel('Amplitude')
#plt.title('Waveform')
#plt.legend()
#plt.grid()
#plt.show()

with open("points2.txt", "w") as f:
    for i in range(len(moving_averages[0:10000])):
        f.write(str(time[i]) + " " + str(moving_averages[i]/np.max(moving_averages)) + "\n")

sigma = 75  # Increase this for smoother transitions
smoother_waveform = gaussian_filter1d(moving_averages[0:10000]/np.max(moving_averages), sigma=sigma)

#plt.plot(time[0:10000], smoother_waveform, label='Waveform', linewidth=1)
#plt.xlabel('Time [s]')
#plt.ylabel('Amplitude')
#plt.title('Waveform')
#plt.legend()
#plt.grid()
#plt.show()

#print("Starting...")
with open("points.txt", "w") as f:
    for i in range(len(smoother_waveform)):
        f.write(str(time[i]) + " " + str(smoother_waveform[i]) + "\n")

#print("Done!")
