#!/bin/bash
# This script will read file list then will grap stat about: NumCommits
# and Numcontributors
# Script by Abdulrhman Alkhanifer, 7-30-2012

#set -x verbose #echo on

file="myfilelist.txt"
#cat $file | od -c
#${filename/$'\n'/} to remove '\n' from the filename

while IFS= read -r filename
do
#echo $string1 | tr "\n" " "
var="$(echo ${filename/$'\r'/})"
#var="$(echo $filename | grep \r )"

# display $line or do somthing with $line
git log --since='2008-01-01' --until='2010-12-3' --oneline -- ${var} |wc -l
done <"$file"
