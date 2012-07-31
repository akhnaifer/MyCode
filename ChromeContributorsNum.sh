#!/bin/bash
# This script will read file list then will grap stat about: Numcontributors
# Script by Abdulrhman Alkhanifer, 7-30-2012

file="myfilelist1.txt"

while IFS= read -r filename
do
# display $line or do somthing with $line
var="$(echo ${filename/$'\r'/})"
git log --since='2008-01-01' --until='2010-12-3' --pretty=format:%ae -- $var | sort -u | wc -l

done <"$file"
