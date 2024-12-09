import cv2
import time,  math, numpy as np
import gesture_recognition as htm
from pynput.mouse import Button, Controller
from screeninfo import get_monitors


main_monitor = None
for monitor in get_monitors():
    if monitor.is_primary:
        main_monitor = monitor
        break

screen_width, screen_height = main_monitor.width, main_monitor.height

wCam, hCam = 640, 480
cap = cv2.VideoCapture(0)
cap.set(3,wCam)
cap.set(4,hCam)
pTime = 0
#cTime = 0

detector = htm.HandDetector(maxHands=1, detectionCon=0.85, trackCon=0.8)


tipIds = [4, 8, 12, 16, 20]
mode = ''
active = 0
wait_time = 0.1
scroll_amount = 1

counter = 0
button_left_down, button_right_down = False, False
mouse = Controller()
while True:
    success, img = cap.read()
    img = detector.findHands(img)
    lmList = detector.findPosition(img, draw=False)
    fingers = []
    counter += 1

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


        if len(fingers) > 0 and fingers == [0, 1, 0, 0, 0]:
            if mode != "left_click":
                print("left click detected")
                mode = "left_click"
        elif len(fingers) > 0 and fingers == [1, 1, 1, 0, 0]:
            if len(fingers) > 0 and mode != "scroll_down":
                print("scroll down detected")
            mode = "scroll_down"
        elif len(fingers) > 0 and fingers == [0, 1, 1, 0, 0]:
            if mode != "scroll_up":
                print("scroll up detected")
            mode = "scroll_up"
        elif len(fingers) > 0 and fingers == [0, 0, 0, 0, 0]:
            if mode != "right_click":
                print("right click detected")
            mode = "right_click"
        else:
            mode = "N"
    else:
        mode = "N"



    if len(fingers) > 0 and fingers[0] == 1:
        if button_left_down:
            mouse.release(Button.left)
            button_left_down = False
        elif button_right_down:
            mouse.release(Button.right)
            button_right_down = False

    if len(fingers) > 0 and fingers[1] == 1:
        time.sleep(wait_time)
        cam_x, cam_y = lmList[8][1], lmList[8][2]
        pos_x = int(((cam_x - 60) / 460) * screen_width)
        pos_y = int(((cam_y - 60) / 240) * screen_height)

        try:
            mouse.position = (pos_x, pos_y)
        except Exception as e:
            print(e)
            pass

    if mode == "left_click":
        if fingers[:2] == [0, 1] and not button_left_down:
            time.sleep(wait_time)
            mouse.press(Button.left)
            button_left_down = True
    elif mode == "right_click" and not button_right_down:
        if fingers == [0, 0, 0, 0, 0]:
            time.sleep(wait_time)
            mouse.press(Button.right)
            button_right_down = True
    elif mode == "scroll_down":
        if fingers == [1, 1, 1, 0, 0]:
            time.sleep(wait_time)
            position = mouse.position
            position = (position[0], position[1] + scroll_amount)
            mouse.scroll(0, -scroll_amount)
    elif mode == "scroll_up":
        if fingers == [0, 1, 1, 0, 0]:
            time.sleep(wait_time)
            mouse.scroll(0, scroll_amount)



    cTime = time.time()
    fps = 1/((cTime + 0.01)-pTime)
    pTime = cTime

    cv2.putText(img,f'FPS:{int(fps)}',(480,50), cv2.FONT_ITALIC,1,(255,0,0),2)
    cv2.imshow('Hand LiveFeed',img)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

    def putText(mode,loc = (250, 450), color = (0, 255, 255)):
        cv2.putText(img, str(mode), loc, cv2.FONT_HERSHEY_COMPLEX_SMALL,
                    3, color, 3)