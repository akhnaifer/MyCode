#!/bin/bash
# This script will read file list then will grap stat about: NumCommits
# and Numcontributors
# Script by Abdulrhman Alkhanifer, 7-30-2012

file="myfilelist.txt"

while IFS= read -r filename
do
echo $filename
# display $line or do somthing with $line
x= git log --since='2008-01-01' --until='2010-12-3' --oneline -- ${filename} |wc -l
done <"$file"
