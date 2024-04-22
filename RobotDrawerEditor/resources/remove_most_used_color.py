from PIL import Image


image_name = input("image name: ")
image = Image.open(image_name + ".png").convert('RGBA')
pixeldata = list(image.getdata())

used_colors = {}

for pixel in pixeldata:
    if pixel[3] != 0:
        if pixel[:3] not in used_colors:
            used_colors[pixel[:3]] = 1
        else:
            used_colors[pixel[:3]] += 1

most_used_color = ([], 0)

for k, v in used_colors.items():
    if v > most_used_color[1]:
        most_used_color = (k, v)

for i, pixel in enumerate(pixeldata):
    if pixel[:3] == most_used_color[0]:
        # print("pixeldata[i] before:", pixeldata[i])
        pixeldata[i] = (*most_used_color[0], 0)
        # print("pixeldata[i] after:", pixeldata[i], "\n----------------")

image.putdata(pixeldata)
image.save(image_name + "_bg.png")
