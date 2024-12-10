import cv2
import time
import gesture_recognition as htm
from pynput.mouse import Button, Controller
from screeninfo import get_monitors
import pyvirtualcam
import keyboard

# Get screen dimensions
main_monitor = None
for monitor in get_monitors():
    if monitor.is_primary:
        main_monitor = monitor
        break

screen_width, screen_height = main_monitor.width, main_monitor.height

# Camera settings
wCam, hCam = 640, 480
cap = cv2.VideoCapture(0)
cap.set(3, wCam)
cap.set(4, hCam)

# Hand detector initialization
detector = htm.HandDetector(maxHands=1, detectionCon=0.85, trackCon=0.8)

# Gesture and mouse settings
tipIds = [4, 8, 12, 16, 20]
mode = ''
wait_time = 0.1
scroll_amount = 1
button_left_down, button_right_down = False, False
mouse = Controller()

# Initialize virtual camera
with pyvirtualcam.Camera(width=wCam, height=hCam, fps=30, fmt=pyvirtualcam.PixelFormat.RGB) as cam:
    print(f"Virtual camera started: {cam.device}")

    while True:
        success, img = cap.read()
        if not success:
            print("Error: Unable to access camera.")
            break

        # Hand detection and gesture recognition
        img = detector.findHands(img)
        lmList = detector.findPosition(img, draw=False)
        fingers = []

        if len(lmList) != 0:
            #Thumb
            if lmList[tipIds[0]][1] > lmList[tipIds[0 -1]][1]:
                if lmList[tipIds[0]][1] >= lmList[tipIds[0] - 1][1]:
                    fingers.append(1)
                else:
                    fingers.append(0)
            elif lmList[tipIds[0]][1] < lmList[tipIds[0 -1]][1]:
                if lmList[tipIds[0]][1] <= lmList[tipIds[0] - 1][1]:
                    fingers.append(1)
                else:
                    fingers.append(0)
            for id in range(1,5):
                if lmList[tipIds[id]][2] < lmList[tipIds[id] - 2][2]:
                    fingers.append(1)
                else:
                    fingers.append(0)

            # Gesture detection
            if fingers == [0, 1, 0, 0, 0]:
                if mode != "left_click":
                    print("left click detected")
                    mode = "left_click"
            elif fingers == [1, 1, 1, 0, 0]:
                if mode != "scroll_down":
                    print("scroll down detected")
                    mode = "scroll_down"
            elif fingers == [0, 1, 1, 0, 0]:
                if mode != "scroll_up":
                    print("scroll up detected")
                    mode = "scroll_up"
            elif fingers == [0, 0, 0, 0, 0]:
                if mode != "right_click":
                    print("right click detected")
                    mode = "right_click"
       
            else:
                mode = "N"

        # Cursor movement logic
        if len(fingers) > 0 and fingers[1] == 1:
            time.sleep(wait_time)
            cam_x, cam_y = lmList[8][1], lmList[8][2]
            pos_x = int(((cam_x - 60) / 460) * screen_width)
            pos_y = int(((cam_y - 60) / 240) * screen_height)

            try:
                mouse.position = (pos_x, pos_y)
            except Exception as e:
                print(e)

        # Mouse actions
        if mode == "left_click" and not button_left_down:
            mouse.press(Button.left)
            button_left_down = True
        elif mode == "right_click" and not button_right_down:
            mouse.press(Button.right)
            button_right_down = True
        elif mode == "scroll_down":
            mouse.scroll(0, -scroll_amount)
        elif mode == "scroll_up":
            mouse.scroll(0, scroll_amount)
    
        # Release buttons
        if len(fingers) > 0 and fingers[0] == 1:
            if button_left_down:
                mouse.release(Button.left)
                button_left_down = False
            elif button_right_down:
                mouse.release(Button.right)
                button_right_down = False
     

        # Send the frame to the virtual camera
        frame_rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        cam.send(frame_rgb)
        cam.sleep_until_next_frame()

        # Exit on 'q'
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

# Release resources
cap.release()

