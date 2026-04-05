import { Component, input, effect, ElementRef, viewChild, afterNextRender, signal, OnDestroy, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import * as L from 'leaflet';

@Component({
  selector: 'app-tour-map',
  standalone: true,
  template: '<div #mapContainer style="height: 100%; width: 100%;"></div>',
  styles: [':host { display: block; height: 400px; }']
})
export class TourMapComponent implements OnDestroy {
  routeCoordinates = input<string>('');
  fromLocation = input<string>('');
  toLocation = input<string>('');

  mapContainer = viewChild<ElementRef>('mapContainer');

  constructor() {
    afterNextRender(() => {
      this.initMap();
    });

    effect(() => {
      const coords = this.routeCoordinates();
      this.updateRoute(coords);
    });
  }

  private map: L.Map | null = null;
  private routeLayer: L.LayerGroup | null = null;

  private initMap() {
    const container = this.mapContainer()?.nativeElement;
    if (!container) return;
    delete (L.Icon.Default.prototype as any)._getIconUrl;
    L.Icon.Default.mergeOptions({
      iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
      iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
      shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
    });

    this.map = L.map(container).setView([47.5, 13.05], 7); // Austria center

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(this.map);

    this.routeLayer = L.layerGroup().addTo(this.map);

    this.updateRoute(this.routeCoordinates());
  }

  private updateRoute(coordsJson: string) {
    if (!this.map || !this.routeLayer) return;

    this.routeLayer.clearLayers();

    if (!coordsJson) return;

    try {
      const coordinates: number[][] = JSON.parse(coordsJson);
      if (!coordinates || coordinates.length === 0) return;
      const latLngs: L.LatLngExpression[] = coordinates.map(
        c => [c[1], c[0]] as L.LatLngExpression
      );
      const polyline = L.polyline(latLngs, {
        color: '#2563eb',
        weight: 4,
        opacity: 0.8
      });
      this.routeLayer.addLayer(polyline);
      const startIcon = L.divIcon({
        html: '<div style="background:#2563eb;width:12px;height:12px;border-radius:50%;border:2px solid white;box-shadow:0 1px 3px rgba(0,0,0,0.3)"></div>',
        className: '',
        iconSize: [16, 16],
        iconAnchor: [8, 8]
      });
      const startMarker = L.marker(latLngs[0], { icon: startIcon });
      startMarker.bindPopup(`<b>Start:</b> ${this.fromLocation()}`);
      this.routeLayer.addLayer(startMarker);
      const endIcon = L.divIcon({
        html: '<div style="background:#06b6d4;width:12px;height:12px;border-radius:50%;border:2px solid white;box-shadow:0 1px 3px rgba(0,0,0,0.3)"></div>',
        className: '',
        iconSize: [16, 16],
        iconAnchor: [8, 8]
      });
      const endMarker = L.marker(latLngs[latLngs.length - 1], { icon: endIcon });
      endMarker.bindPopup(`<b>End:</b> ${this.toLocation()}`);
      this.routeLayer.addLayer(endMarker);
      this.map.fitBounds(polyline.getBounds(), { padding: [30, 30] });
    } catch (e) {
      console.warn('Failed to parse route coordinates:', e);
    }
  }

  ngOnDestroy() {
    this.map?.remove();
    this.map = null;
    this.routeLayer = null;
  }
}
