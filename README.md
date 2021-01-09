# Puppeteer Automation
This project aims to build an application capable of recording and replaying interaction with Chromium/Google Chrome browser using [Puppeteer](https://pptr.dev/).

The project consists of two parts that communicate with each other using [ZeroMQ](https://zeromq.org/).

---
## Warning
This project is currently being developed. 

---

## Docs
My thesis containing a comprehensive documentation is unfortunately only available in the Czech language. If you are interested, feel free: [text/bp.pdf](text/bp.pdf).

## Backend
This part is located in [Backend/](backend/) directory. It is implemented in Node.js. The main thing it is capable of is capturing (some) events from the viewport. It can also capture some events with a Chrome window such as opening a new tab.

## Frontend
This is a C# application, it uses WinForms for an old-school UI. It looks something like this: 

![Frontend application](https://i.imgur.com/tfcbJz7.png)

As you can see it can do various things like generating Puppeteer code and replaying recorded actions.