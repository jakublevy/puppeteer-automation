# Puppeteer Automation
This project aims to build an application capable of recording and replaying interaction with Chromium/Google Chrome browser using [Puppeteer](https://pptr.dev/).

The project consists of [Backend](#Backend) and [Frontend](#Frontend) that communicate with each other using [ZeroMQ](https://zeromq.org/).

---

## Backend
This part is located in [Backend/](Backend/) directory. It is implemented in Node.js. The main thing it is capable of is capturing certain events from the viewport. It can also capture some events with a Chrome window such as opening a new tab.

## Frontend
This is a C# application located in [Frontend/](Frontend/), it uses WinForms for an old-school UI. It looks something like this: 

![Frontend application](https://i.imgur.com/tfcbJz7.png)

As you can see it can do various things like generating Puppeteer code and replaying recorded actions.

## Docs
My thesis containing a comprehensive documentation is unfortunately only available in the Czech language. If you are interested, feel free: [Text/bp.pdf](Text/bp.pdf).

## Project Directory Structure

| Description    | Location |
| ------------- |-------------:|
| [Backend](#Backend) | [Backend/](Backend/) |
| [Frontend](#Frontend) | [Frontend/](Frontend/) |
| Comprehensive<br>documentation | [Text/](Text/)
| Node.js<br>(un)installation scripts | [Scripts/](Scripts/) |