import type { Metadata } from "next";
import { Provider as JotaiProvider } from 'jotai';
import {Header} from "@/components/header/Header"
import localFont from "next/font/local";
import "./globals.css";
import {Toaster} from "@/components/ui/toaster";

const geistSans = localFont({
  src: "./fonts/GeistVF.woff",
  variable: "--font-geist-sans",
  weight: "100 900",
});
const geistMono = localFont({
  src: "./fonts/GeistMonoVF.woff",
  variable: "--font-geist-mono",
  weight: "100 900",
});

export const metadata: Metadata = {
  title: "My Muse",
  description: "自分の写真を登録し、VR空間の美術館で展示・鑑賞できるウェブアプリ。ユーザー自身が美術館のような空間で写真を閲覧し、作品を楽しむことができます。",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="jp">
      <body
        className={`${geistSans.variable} ${geistMono.variable} antialiased`}
      >
        <JotaiProvider>
          <Header/>
          {children}
          <Toaster />
        </JotaiProvider>
      </body>
    </html>
  );
}
