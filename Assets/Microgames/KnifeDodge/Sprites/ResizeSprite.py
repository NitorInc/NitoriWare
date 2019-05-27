'''
Python script to crunch sprite assets in Unity by a constant value (for reducing filesize with big images
This automatically applies to .meta files, so nothing changes in game, and it works on Multiple sprite assets as well.
PIL (Python Image Library) needs to be installed to use this. 

@author: Brian Intile
'''

import os

from PIL import Image
import PIL


def get_immediate_subdirectories(directory):
    return [name for name in os.listdir(directory)
            if os.path.isdir(os.path.join(directory, name))]

def browse_for_traits(directory, recurse, mult):
    for dir_0 in os.listdir(directory):
        if (os.path.isfile(directory + u'\\' + dir_0) & ((dir_0.split('.')[-1].lower() == u'png'.lower()) | (dir_0.split('.')[-1].lower() == u'jpg'.lower()))):
            change_value_names(directory + '\\' + dir_0, mult)
    if (recurse):
        directories = get_immediate_subdirectories(directory)
        for x in range (0, len(directories)):
            browse_for_traits(directory + '\\' + directories[x], recurse, mult)

def count_leading_whitespace(line):
    for x in range(len(line)):
        if (line[x] != ' '):
            return x
    return len(line)

def multiply_bounds(contents, mult):
    in_rect = False
    whitespace_count = 0
    for x in range(len(contents)):
        if (in_rect):
            if (count_leading_whitespace(contents[x]) == whitespace_count):
                category = contents[x].split(':')[0].split(' ')[-1]
                if (category == 'x' or category == 'y' or category == 'width' or category == 'height'):
                    number = contents[x].split(' ')[-1]
                    contents[x] = contents[x].replace(number, str(int(float(number) * mult)))
            else:
                in_rect = False
        elif (contents[x].split(' ')[-1] == 'rect:'):
            whitespace_count = count_leading_whitespace(contents[x]) + 2
            in_rect = True 
        elif (contents[x][count_leading_whitespace(contents[x]):].split(':')[0] == "spritePixelsToUnits"):
            number = contents[x].split(' ')[-1]
            result = float(float(number) * mult)
            if (result % 1 == 0):
                result = int(result)
            contents[x] = contents[x].replace(number, str(result))
    return contents

def change_value_names(name, mult):
    if not (os.path.isfile(name)):
        print("File " + name + " not found")
        return
    img = Image.open(name)
    width = int(float(img.size[0]) * mult)
    height = int(float(img.size[1] * mult))
    img = img.resize((width, height), PIL.Image.ANTIALIAS)
    img.save(name)
    
    if not (os.path.isfile(name + '.meta')):
        print("File " + name + " has no associated .meta file")
        return
    f = open(name + '.meta', 'r')
    contents = multiply_bounds(f.read().split('\n'), mult)
    f.close()
    f = open(name + '.meta', 'w')
    
    f.write(contents[0])
    for x in range(1, len(contents)):
        f.write('\n' + contents[x])
    f.close()

file_path = os.path.dirname(os.path.realpath(__file__))
#file_path = str(file_path, 'utf8')

while True:
    command = input("Unity Sprite Resizer, this will rescale your image(s) and adjust the .meta accordingly for split sprites."
    + "\nEnter an image filename to resize that sprite by a constant."
    + "\nEnter 'a' to resize all sprites in the folder by a constant. (pngs or jpgs only)"
    + "\nEnter 'r' to resize all sprites in the folder AND all subfolders by a constant. (pngs or jpgs only)"
    + "\nEnter 'q' to quit:\n")
    if command == 'q':
        break
    mult = float(input("Enter the number you wish to multiply each dimension by (use values <1 for crunching assets):\n"))
    
    if command == 'a':
        browse_for_traits(file_path, False, mult)
    elif command == 'r':
        browse_for_traits(file_path, True, mult)
    else:
        change_value_names(file_path + '\\' + command, mult)